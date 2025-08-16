using Application.Consumers;
using Domain.Event;
using Infrastructure.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration cfg)
        {
            var uri = cfg["RabbitMq:Uri"] ?? Environment.GetEnvironmentVariable("CLOUDAMQP_URL");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<RegisterProductConsumer>();
                x.AddConsumer<ProductDeletedConsumer>();

                x.AddConsumer<RegisterOrderConsumer>();
                x.AddConsumer<OrderDeletedConsumer>();

                x.AddConsumer<RegisterUserConsumer>();

                x.AddConsumer<CartAddConsumer>();

                x.AddConsumer<PaymentProcessConsumer>();

                x.UsingRabbitMq((context, cfgRabbit) =>
                {
                    cfgRabbit.Host(new Uri(uri));

                    const ushort prefetch = 128;
                    const int concurrency = 16;

                    cfgRabbit.ReceiveEndpoint("product-registration-commands", e =>
                    {
                        e.PrefetchCount = prefetch;
                        e.ConcurrentMessageLimit = concurrency;
                        e.ConfigureConsumer<RegisterProductConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.UseInMemoryOutbox();
                    });

                    cfgRabbit.ReceiveEndpoint("product-deleted-queue", e =>
                    {
                        e.PrefetchCount = prefetch;
                        e.ConcurrentMessageLimit = concurrency;
                        e.ConfigureConsumer<ProductDeletedConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.UseInMemoryOutbox();
                    });

                    cfgRabbit.ReceiveEndpoint("order-registration-commands", e =>
                    {
                        e.PrefetchCount = prefetch;
                        e.ConcurrentMessageLimit = concurrency;
                        e.ConfigureConsumer<RegisterOrderConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.UseInMemoryOutbox();
                    });

                    cfgRabbit.ReceiveEndpoint("order-deleted-queue", e =>
                    {
                        e.ConfigureConsumer<OrderDeletedConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.UseInMemoryOutbox();
                    });

                    cfgRabbit.ReceiveEndpoint("user-registration-commands", e =>
                    {
                        e.PrefetchCount = prefetch;
                        e.ConcurrentMessageLimit = concurrency;
                        e.ConfigureConsumer<RegisterUserConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.UseInMemoryOutbox();
                    });

                    cfgRabbit.ReceiveEndpoint("cart-add-commands", e =>
                    {
                        e.PrefetchCount = prefetch;
                        e.ConcurrentMessageLimit = concurrency;
                        e.ConfigureConsumer<CartAddConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.Bind("queue:cart-add-commands");
                        e.UseInMemoryOutbox();
                    });

                    cfgRabbit.ReceiveEndpoint("payment-processs-commands", e =>
                    {
                        e.PrefetchCount = prefetch;
                        e.ConcurrentMessageLimit = concurrency;
                        e.ConfigureConsumer<PaymentProcessConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.UseInMemoryOutbox();
                    });
                });
            });

            return services;
        }
    }
}
