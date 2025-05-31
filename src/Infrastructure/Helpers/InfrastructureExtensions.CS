using Application.Consumers;
using Domain.Event;
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
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductCreatedConsumer>();
                x.AddConsumer<ProductDeletedConsumer>();

                x.AddConsumer<OrderCreatedConsumer>();
                x.AddConsumer<OrderDeletedConsumer>();

                x.UsingRabbitMq((context, cfgRabbit) =>
                {
                    var rabbitHost = cfg["RabbitMq:Host"];
                    var rabbitUser = cfg["RabbitMq:User"];
                    var rabbitPass = cfg["RabbitMq:Pass"];

                    cfgRabbit.Host(cfg["RabbitMq:Host"], h =>
                    {
                        h.Username(rabbitUser);
                        h.Password(rabbitPass);
                    });

                    cfgRabbit.ReceiveEndpoint("product-created-queue", e =>
                    {
                        e.ConfigureConsumer<ProductCreatedConsumer>(context);

                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                    });

                    cfgRabbit.ReceiveEndpoint("product-deleted-queue", e =>
                    {
                        e.ConfigureConsumer<ProductDeletedConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                    });

                    cfgRabbit.ReceiveEndpoint("order-created-queue", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                    });

                    cfgRabbit.ReceiveEndpoint("order-deleted-queue", e =>
                    {
                        e.ConfigureConsumer<OrderDeletedConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                    });
                });
            });

            return services;
        }
    }
}
