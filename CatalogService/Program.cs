using Microsoft.EntityFrameworkCore;
using CatalogService.Data;
using CatalogService.Middleware;
using CatalogService.Interfaces;
using CatalogService.Services;

namespace CatalogService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("CatalogDb"));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddLogging();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IItemService, ItemService>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
