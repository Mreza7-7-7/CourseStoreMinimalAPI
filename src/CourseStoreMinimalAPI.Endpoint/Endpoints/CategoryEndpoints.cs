using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseStoreMinimalAPI.Endpoint.EndPoints;

public static class Categories
{
    public static WebApplication MapCategories(this WebApplication app, string prefix)
    {
        var MGCategories = app.MapGroup(prefix);
        MGCategories.MapGet("/", Getlist).CacheOutput(c => { c.Expire(TimeSpan.FromMinutes(20)).Tag("categories"); });
        MGCategories.MapGet("/{id:int}", GetById);
        MGCategories.MapPost("/", Insert);
        MGCategories.MapPut("/{id:int}", Update);
        MGCategories.MapDelete("/{id:int}", Delete);
        return app;
    }
    static async Task<Ok<List<Category>>> Getlist(CategoryService categoryService)
    {
        var result = await categoryService.GetCategoriesAsync();
        return TypedResults.Ok<List<Category>>(result);
    }
    static async Task<Results<NotFound, Ok<Category>>> GetById(CategoryService categoryService, int id)
    {
        var result = await categoryService.GetCategoriesAsync(id);
        return result == null ? TypedResults.NotFound() : TypedResults.Ok<Category>(result);
    }
    static async Task<Created<Category>> Insert(CategoryService categoryService, IOutputCacheStore outputCacheStore, Category category)
    {
        var result = categoryService.Insert(category);
        await outputCacheStore.EvictByTagAsync("categories", default);
        return TypedResults.Created($"/categories/{result}", category);
    }
    static async Task<Results<NotFound, NoContent>> Update(Category category, CategoryService categoryService, IOutputCacheStore outputCacheStore, int id)
    {
        if (!await categoryService.Exist(id))
            return TypedResults.NotFound();
        else
        {
            categoryService.UpdateAsync(category);
            await outputCacheStore.EvictByTagAsync("categories", default);
            return TypedResults.NoContent();
        }
    }
    static async Task<Results<NoContent, NotFound>> Delete(CategoryService categoryService, IOutputCacheStore outputCacheStore, int id)
    {
        if (!await categoryService.Exist(id))
            return TypedResults.NotFound();
        else
        {
            await categoryService.Delete(id);
            await outputCacheStore.EvictByTagAsync("categories", default);
            return TypedResults.NoContent();
        }
    }
}

