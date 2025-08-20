using Application.Mappings.OrderMapping;
using Application.Services.CartServices;
using Application.Services.OrderServices;
using Application.Services.PaymentServices;
using Application.Services.ProductServices;
using Application.Services.UserServices;
using Domain.Interfaces.CartInterface;
using Domain.Interfaces.OrderInterface;
using Domain.Interfaces.PaymentInterface;
using Domain.Interfaces.ProductInterface;
using Domain.Interfaces.UnitOfWork;
using Domain.Interfaces.UserInterface;
using EcommerceSync.Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Repositories.CartRepository;
using Infrastructure.Repositories.OrderRepository;
using Infrastructure.Repositories.PaymentRepository;
using Infrastructure.Repositories.ProductRepository;
using Infrastructure.Repositories._unitOfWorkRepository;
using Infrastructure.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.EndpointCache;
using Microsoft.Extensions.Options;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddScoped<IFakePaymentGatewayClient>(sp => sp.GetRequiredService<FakePaymentGatewayClient>());
            builder.Services.AddSingleton<FakePaymentGatewayClient>();
            builder.Services.AddSingleton<IEndpointCache, EndpointCache>();

            builder.Services.AddMessaging(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(OrderProfile).Assembly);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<QueueMetricsOptions>(builder.Configuration.GetSection("QueueMetrics"));

            builder.Services.AddHttpClient("RabbitMQManagement", (sp, http) =>
            {
                var opt = sp.GetRequiredService<IOptions<QueueMetricsOptions>>().Value;
                http.BaseAddress = new Uri(opt.BaseUrl.TrimEnd('/')); 
                http.Timeout = TimeSpan.FromSeconds(10);
            });

            builder.Services.AddHostedService<QueueMetricsCollector>();

            var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? builder.Configuration["ConnectionStrings:DefaultConnection"]
                          ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

            builder.Services.AddDbContextPool<EcommerceSyncDbContext>(opt =>
                opt.UseNpgsql(connStr, npg =>
                    npg.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null)));

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
