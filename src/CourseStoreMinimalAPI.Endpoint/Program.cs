using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
//services
builder.Services.AddScoped<CategoryService>();
builder.Services.AddOutputCache();
string connectionString = builder.Configuration.GetConnectionString("CourseCnn");
builder.Services.AddDbContext<CourseDbContext>(c =>
{
    c.UseSqlServer(connectionString);
});
var app = builder.Build();


//Pipeline
app.UseOutputCache();
app.MapScalarApiReference();
app.MapGet("/", () => "Hello World!");
app.MapGet("/categories", async (CategoryService categoryService) =>
{
    var result = await categoryService.GetCategoriesAsync();
    return Results.Ok<List<Category>>(result);
}).CacheOutput(c =>
{
    c.Expire(TimeSpan.FromMinutes(20)).Tag("categories");
});
app.MapGet("/categories/{id:int}", async (CategoryService categoryService, int id) =>
{
    var result = await categoryService.GetCategoriesAsync(id);
    return result == null ? Results.NotFound() : Results.Ok<Category>(result);
});
app.MapPost("/categories", async (CategoryService categoryService, IOutputCacheStore outputCacheStore, Category category) =>
{
    var result = categoryService.Insert(category);
    await outputCacheStore.EvictByTagAsync("categories", default);
    return Results.Created($"/categories/{result}", category);
});
app.MapPut("/categories/{id:int}", async (Category category, CategoryService categoryService, IOutputCacheStore outputCacheStore, int id) =>
{
    if (!await categoryService.Exist(id))
        return Results.NotFound();
    else
    {
        categoryService.UpdateAsync(category);
        await outputCacheStore.EvictByTagAsync("categories", default);
        return Results.NoContent();
    }
});
app.Run();
