// Infrastructure/Consumers/OrderCreatedConsumer.cs
using Domain.Entities.OrderEntity;
using Domain.Entities.OrderItemEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infrastructure.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        private static readonly Channel<string> _logChannel =
            Channel.CreateUnbounded<string>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });

        private static readonly string LogFilePath;

        static OrderCreatedConsumer()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var directory = @"N:\Projeto de Pesquisa\Resultados\MONITORAMENTO\1500 USUARIOS RABBIT MQ";
            var fileName = $"tempo_processamento_{timestamp}.txt";

            LogFilePath = Path.Combine(directory, fileName);

            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch
            {
            }

            _ = Task.Run(async () =>
            {
                await using var fs = new FileStream(
                    LogFilePath,
                    FileMode.Append,
                    FileAccess.Write,
                    FileShare.Read,
                    bufferSize: 4096,
                    useAsync: true);

                using var sw = new StreamWriter(fs);

                await foreach (var line in _logChannel.Reader.ReadAllAsync())
                {
                    try
                    {
                        await sw.WriteLineAsync(line);
                        await sw.FlushAsync();
                    }
                    catch
                    {
                    }
                }
            });
        }

        public OrderCreatedConsumer(
            IUnitOfWork unitOfWork,
            ILogger<OrderCreatedConsumer> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var evt = context.Message;

            var swTotal = Stopwatch.StartNew();

            var startedAt = DateTime.UtcNow;
            _logger.LogInformation(
                "Recebido OrderCreatedEvent: CorrelationId={CorrelationId} em {Timestamp}",
                evt.CorrelationId,
                startedAt.ToString("O"));

            var order = new Order
            {
                DateOfOrder = evt.DateOfOrder,
                OrderIdentifier = evt.OrderIdentifier,
                UserId = evt.UserId,
                Status = evt.Status
            };

            float total = 0f;
            foreach (var itemEvt in evt.Items)
            {
                var produto = await _unitOfWork.Products.GetByIdAsync(itemEvt.ProductId);
                if (produto == null)
                {
                    _logger.LogError(
                        "Produto com ID {ProductId} não encontrado para Order {CorrelationId}.",
                        itemEvt.ProductId,
                        evt.CorrelationId);
                    throw new Exception($"Produto com ID {itemEvt.ProductId} não encontrado.");
                }

                produto.Stock -= itemEvt.Quantity;
                _unitOfWork.Products.Update(produto);

                await _unitOfWork.CommitAsync();

                total += produto.Price * itemEvt.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = itemEvt.ProductId,
                    Quantity = itemEvt.Quantity,
                    Product = produto
                };
                order.Items.Add(orderItem);
            }

            order.Total = total;
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitAsync();

            swTotal.Stop();
            var elapsedMs = swTotal.Elapsed.TotalMilliseconds;

            var finishedAt = DateTime.UtcNow;
            _logger.LogInformation(
                "Pedido persistido no banco. OrderId={OrderId}, CorrelationId={CorrelationId}, End={EndTime}",
                order.Id,
                evt.CorrelationId,
                finishedAt.ToString("O"));

            var line = $"{evt.CorrelationId};{startedAt:O};{finishedAt:O};{elapsedMs}";
            _logChannel.Writer.TryWrite(line);
        }
    }
}
