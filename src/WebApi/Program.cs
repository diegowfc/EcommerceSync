using Application.Mappings.OrderMapping;
using Application.Services.OrderServices;
using Application.Services.ProductServices;
using Application.Services.UserServices;
using Domain.Interfaces.OrderInterface;
using Domain.Interfaces.ProductInterface;
using Domain.Interfaces.UnitOfWork;
using Domain.Interfaces.UserInterface;
using EcommerceSync.Infrastructure.Data;
using Infrastructure.Repositories.OrderRepository;
using Infrastructure.Repositories.ProductRepository;
using Infrastructure.Repositories.UoWRepository;
using Infrastructure.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IUserService, UserService>();


            builder.Services.AddAutoMapper(typeof(OrderProfile).Assembly);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
