
using E_Commerce.Web.CustomMiddlewares;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.DataSeeding;
using ECommerce.Persistence.Data.DbContexts;
using ECommerce.Persistence.Repositories;
using ECommerce.Services;
using ECommerce.Services.Abstraction;
using ECommerce.Services.MappingProfiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Register DI Container

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDataInitializer, DataInitializer>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            builder.Services.AddScoped<IBasketService, BasketService>();

            builder.Services.AddScoped<ICacheRepository, CacheRepository>();

            builder.Services.AddScoped<ICacheService, CacheService>();



            builder.Services.AddAutoMapper(typeof(ServiceAssemblyReference).Assembly);


            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResposeFactory.GenerateApiValidationRespose;
            });

            #endregion


            var app = builder.Build();

            await app.MigrateDataBaseAsync();

            await app.SeedDataAsync();


            #region Configure PipLine [Middleware]

            //Custom Middleware

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            await app.RunAsync();
        }
    }
}
