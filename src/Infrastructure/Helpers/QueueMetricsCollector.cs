using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Helpers;

public sealed class QueueMetricsCollector(
    IHttpClientFactory httpClientFactory,
    IOptions<QueueMetricsOptions> options,
    ILogger<QueueMetricsCollector> log
) : BackgroundService
{
    private readonly QueueMetricsOptions _opt = options.Value;

    // records aninhados
    private sealed record Details(double rate);
    private sealed record MsgStats(Details? publish_details, Details? ack_details, Details? deliver_get_details);
    private sealed record QueueView(
        string name,
        int messages,
        int messages_ready,
        int messages_unacknowledged,
        MsgStats? message_stats
    );

    // Estado por fila para saber se mudou
    private sealed record LastSample(
        int messages, int ready, int unacked,
        double pub, double ack, double del,
        DateTime lastWriteUtc
    );

    private readonly Dictionary<string, LastSample> _last = new(StringComparer.Ordinal);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Directory.CreateDirectory(_opt.OutputDirectory);

        var csvPath = _opt.NewFilePerRun
            ? Path.Combine(_opt.OutputDirectory, $"queue-metrics_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv")
            : Path.Combine(_opt.OutputDirectory, "queue-metrics.csv");

        EnsureHeader(csvPath);

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(Math.Max(1, _opt.PollSeconds)));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            foreach (var q in _opt.Queues)
            {
                try
                {
                    var view = await ReadQueueAsync(q, stoppingToken);
                    if (view is null) continue;

                    var publishRate = view.message_stats?.publish_details?.rate ?? 0d;
                    var ackRate = view.message_stats?.ack_details?.rate ?? 0d;
                    var deliverRate = view.message_stats?.deliver_get_details?.rate ?? 0d;

                    // 1) pular amostras ociosas (sem backlog e sem tráfego)
                    if (_opt.SkipIdle &&
                        view.messages == 0 && view.messages_ready == 0 && view.messages_unacknowledged == 0 &&
                        publishRate == 0 && ackRate == 0 && deliverRate == 0)
                    {
                        continue;
                    }

                    // 2) pular se não mudou vs. último (com tolerância)
                    var key = view.name;
                    var changed = true;
                    var now = DateTime.UtcNow;
                    if (_opt.OnlyWhenChanged && _last.TryGetValue(key, out var last))
                    {
                        bool eq(int a, int b) => a == b;
                        bool eqd(double a, double b) => Math.Abs(a - b) < 0.001; // tolerância

                        changed = !(eq(view.messages, last.messages) &&
                                    eq(view.messages_ready, last.ready) &&
                                    eq(view.messages_unacknowledged, last.unacked) &&
                                    eqd(publishRate, last.pub) &&
                                    eqd(ackRate, last.ack) &&
                                    eqd(deliverRate, last.del));

                        if (_opt.MinWriteIntervalSeconds > 0 &&
                            (now - last.lastWriteUtc).TotalSeconds < _opt.MinWriteIntervalSeconds)
                        {
                            changed = false;
                        }
                    }
                    if (!changed) continue;

                    double? drainTotal = ackRate > 0 ? view.messages / ackRate : null;
                    double? drainReady = ackRate > 0 ? view.messages_ready / ackRate : null;

                    var line = string.Join(",",
                        DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture),
                        Escape(view.name),
                        view.messages.ToString(CultureInfo.InvariantCulture),
                        view.messages_ready.ToString(CultureInfo.InvariantCulture),
                        view.messages_unacknowledged.ToString(CultureInfo.InvariantCulture),
                        ToCsvNumber(publishRate),
                        ToCsvNumber(ackRate),
                        ToCsvNumber(deliverRate),
                        ToCsvNumber(drainTotal),
                        ToCsvNumber(drainReady)
                    );

                    await File.AppendAllTextAsync(csvPath, line + Environment.NewLine, Encoding.UTF8, stoppingToken);

                    _last[key] = new LastSample(
                        view.messages, view.messages_ready, view.messages_unacknowledged,
                        publishRate, ackRate, deliverRate,
                        now
                    );
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    log.LogWarning(ex, "Falha ao coletar métricas da fila {Queue}", q);
                }
            }
        }
    }

    private void EnsureHeader(string path)
    {
        if (!File.Exists(path))
        {
            var header = "timestamp_utc,queue,messages_total,messages_ready,messages_unacked,publish_rate,ack_rate,deliver_rate,drain_time_total_s,drain_time_ready_s";
            File.WriteAllText(path, header + Environment.NewLine, Encoding.UTF8);
        }
    }

    private static string ToCsvNumber(double? v) =>
        v.HasValue ? v.Value.ToString(CultureInfo.InvariantCulture) : "";

    private static string Escape(string s) =>
        s.Contains(',') ? $"\"{s.Replace("\"", "\"\"")}\"" : s;

    private async Task<QueueView?> ReadQueueAsync(string queue, CancellationToken ct)
    {
        var client = httpClientFactory.CreateClient("RabbitMQManagement");

        var token = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_opt.Username}:{_opt.Password}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

        var v = Uri.EscapeDataString(_opt.VHost);
        var q = Uri.EscapeDataString(queue);

        using var resp = await client.GetAsync($"/api/queues/{v}/{q}", ct);
        if (!resp.IsSuccessStatusCode) return null;

        var json = await resp.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<QueueView>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
