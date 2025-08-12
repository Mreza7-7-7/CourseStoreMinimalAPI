
using AutoMapper;
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Endpoint.Endpoints;
using CourseStoreMinimalAPI.Endpoint.EndPoints;
using CourseStoreMinimalAPI.Endpoint.InfraStructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRequestAndResponses;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
namespace CourseStoreMinimalAPI.Endpoint.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigurServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<CategoryService>();
        builder.Services.AddScoped<TeacherService>();
        builder.Services.AddScoped<CourseService>();
        builder.Services.AddScoped<CommentService>();
        builder.Services.AddOutputCache();
        builder.Services.AddValidatorsFromAssembly(typeof(TeacherRequestValidator).Assembly);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAutoMapper(c =>
        {
            c.AddProfile(new AutoMapperProfile());
        });
        builder.Services.AddScoped<IFileAdapter, LocalFileStorageAdapter>();
        builder.Services.AddOpenApi();
        AddEFCore(builder);
        return builder.Build();
    }
    public static WebApplication ConfigurPipline(this WebApplication app)
    {
        app.UseOutputCache();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        app.UseStaticFiles();
        app.MapGet("/", () => "Hello World!");
        app.MapCategories("/categories");
        app.MapTeachers("/teachers");
        app.MapComments("/comments");
        return app;
    }
    private static void AddEFCore(WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("CourseCnn");
        builder.Services.AddDbContext<CourseStoreDbContext>(c =>
        {
            c.UseSqlServer(connectionString);
        });
    }
}

