
using AutoMapper;
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Endpoint.Endpoints;
using CourseStoreMinimalAPI.Endpoint.EndPoints;
using CourseStoreMinimalAPI.Endpoint.InfraStructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRequestAndResponses;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        builder.Services.AddAuthentication().AddJwtBearer(c =>
        {
            c.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT: Key"]))
            };
        });
        builder.Services.AddAuthorization();
        builder.Services.AddAutoMapper(c =>
        {
            c.AddProfile(new AutoMapperProfile());
        });
        builder.Services.AddScoped<IFileAdapter, LocalFileStorageAdapter>();
        builder.Services.AddOpenApi();
        AddEFCore(builder);
        builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<CourseStoreDbContext>().AddDefaultTokenProviders();
        builder.Services.AddScoped<UserManager<IdentityUser>>();
        builder.Services.AddScoped<SignInManager<IdentityUser>>();
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
        app.UseAuthorization();
        app.MapGet("/", () => "Hello World!");
        app.MapCategories("/categories");
        app.MapTeachers("/teachers");
        app.MapComments("/comments");
        app.MapUsers("users");
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

