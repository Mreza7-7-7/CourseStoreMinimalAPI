using System.Runtime.CompilerServices;
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Endpoint.EndPoints;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace CourseStoreMinimalAPI.Endpoint.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigurService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<CategoryService>();
        builder.Services.AddOutputCache();
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

