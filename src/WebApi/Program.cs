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

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<EcommerceSyncDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

            var host = builder.Configuration["ConnectionStrings:DefaultConnection:Host"];
            //var port = builder.Configuration["ConnectionStrings:DefaultConnection:Port"];
            var db = builder.Configuration["ConnectionStrings:DefaultConnection:Database"];
            var user = builder.Configuration["ConnectionStrings:DefaultConnection:Username"];
            var pwd = builder.Configuration["ConnectionStrings:DefaultConnection:Password"];
            //var connStr = $"Host={host};Port={port};Database={db};Username={user};Password={pwd};"; //LOCAL
            var connStr = $"Host={host};Database={db};Username={user};Password={pwd};SSL Mode=Require;Trust Server Certificate=true"; //SUPABASE
            builder.Services.AddDbContext<DbContext>(o => o.UseNpgsql(connStr));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
