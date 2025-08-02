
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Endpoint.EndPoints;
using CourseStoreMinimalAPI.Endpoint.InfraStructures;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace CourseStoreMinimalAPI.Endpoint.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigurServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<CategoryService>();
        builder.Services.AddOutputCache();
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        AddEFCore(builder);
        return builder.Build();
    }
    public static WebApplication ConfigurPipline(this WebApplication app)
    {
        app.UseOutputCache();
        app.MapScalarApiReference();
        app.MapGet("/", () => "Hello World!");
        app.MapCategories("/categories");
        return app;
    }
    private static void AddEFCore(WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("CourseCnn");
        builder.Services.AddDbContext<CourseDbContext>(c =>
        {
            c.UseSqlServer(connectionString);
        });
    }
}

