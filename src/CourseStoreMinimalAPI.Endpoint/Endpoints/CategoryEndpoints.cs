using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.Endpoint.InfraStructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRequestsAndResponses;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CourseRequestsAndResponses;
using CourseStoreMinimalAPI.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseStoreMinimalAPI.Endpoint.EndPoints;

public static class Categories
{
    static string Cachekey = "categories";
    private static string _prefix;
    public static WebApplication MapCategories(this WebApplication app, string prefix)
    {
        _prefix = prefix;
        var MGCategories = app.MapGroup(prefix);
        MGCategories.MapGet("/", Getlist).CacheOutput(c => { c.Expire(TimeSpan.FromMinutes(20)).Tag(Cachekey); }).AddEndpointFilter<LoggerFilter>();
        MGCategories.MapGet("/{id:int}", GetById);
        MGCategories.MapPost("/", Insert).AddEndpointFilter<ValidationFilter<CategoryRequest>>();
        MGCategories.MapPut("/{id:int}", Update).AddEndpointFilter<ValidationFilter<CategoryRequest>>();
        MGCategories.MapDelete("/{id:int}", Delete);
        return app;
    }
    static async Task<Ok<List<CategoryResponse>>> Getlist(CategoryService categoryService, IMapper mapper)
    {
        var result = await categoryService.GetCategoriesAsync();
        var response = mapper.Map<List<CategoryResponse>>(result);
        return TypedResults.Ok<List<CategoryResponse>>(response);
    }
    static async Task<Results<NotFound, Ok<CategoryResponse>>> GetById(CategoryService categoryService, int id, IMapper mapper)
    {
        var result = await categoryService.GetCategoriesAsync(id);
        var response = mapper.Map<CategoryResponse>(result);
        return result == null ? TypedResults.NotFound() : TypedResults.Ok<CategoryResponse>(response);
    }
    static async Task<Results<Created<CategoryResponse>, ValidationProblem>> Insert(CategoryService categoryService,
                                                        IOutputCacheStore outputCacheStore,
                                                        CategoryRequest categoryRequest,
                                                        IMapper mapper)
    {
        var category = mapper.Map<Category>(categoryRequest);
        var savedEntityId = categoryService.Insert(category);
        await outputCacheStore.EvictByTagAsync(Cachekey, default);
        var respons = mapper.Map<CategoryResponse>(category);
        return TypedResults.Created($"/{_prefix}/{savedEntityId}", respons);
    }
    static async Task<Results<NotFound, NoContent>> Update(CategoryRequest categoryRequest, IMapper mapper, CategoryService categoryService, IOutputCacheStore outputCacheStore, int id)
    {
        if (!await categoryService.Exist(id))
            return TypedResults.NotFound();
        else
        {
            var request = mapper.Map<Category>(categoryRequest);
            await categoryService.UpdateAsync(request);
            await outputCacheStore.EvictByTagAsync(Cachekey, default);
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
            await outputCacheStore.EvictByTagAsync(Cachekey, default);
            return TypedResults.NoContent();
        }
    }
}

