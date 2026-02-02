using ECommerceService.Api.Extensions;
using ECommerceService.Api.Middlewares;
using ECommerceService.Api.Profiles;
using ECommerceService.Api.Repositories;
using ECommerceService.Api.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.OData;

namespace ECommerceService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddOData(opt =>
                opt.Select().Filter().OrderBy().Expand().SetMaxTop(100)
                );

            builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplicationServices();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IStockReservationService, StockReservationService>();

            builder.Services.AddAutoMapper(typeof(ProductMappingProfile));



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
