using AutoMapper;
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.Endpoint.InfraStructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRAR;
using CourseStoreMinimalAPI.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseStoreMinimalAPI.Endpoint.Endpoints;

public static class TeacherEndpoints
{
    static string CacheKey = "Teachers";
    static string TeacherImageFolder = @"Images\Teachers";
    static string DefaultTeacherImageName = "Default.jpg";
    public static string _prefix;
    public static WebApplication MapTeachers(this WebApplication app, string prefix)
    {
        _prefix = prefix;
        var MGTeachers = app.MapGroup(prefix);
        MGTeachers.MapGet("/", Getlist).CacheOutput(c => { c.Expire(TimeSpan.FromMinutes(20)).Tag(CacheKey); });
        MGTeachers.MapGet("/{id:int}", GetById);
        MGTeachers.MapPost("/", Insert).DisableAntiforgery();
        MGTeachers.MapPut("/{id:int}", Update);
        MGTeachers.MapDelete("/{id:int}", Delete);
        return app;
    }
    static async Task<Created<TeacherResponse>> Insert(TeacherService teacherService, IFileAdapter fileAdapter, IOutputCacheStore outputCacheStore, [FromForm] TeacherRequest teacherRequest, IMapper mapper)
    {
        var teacher = mapper.Map<Teacher>(teacherRequest);
        string fileName = DefaultTeacherImageName;
        if (teacherRequest.File is not null)
        {
            fileName = fileAdapter.InsertFile(teacherRequest.File, TeacherImageFolder);
        }
        teacher.ImageUrl = fileName;
        var savedEntityId = teacherService.Insert(teacher);
        await outputCacheStore.EvictByTagAsync(CacheKey, default);
        var respons = mapper.Map<TeacherResponse>(teacher);
        return TypedResults.Created($"/{_prefix}/{savedEntityId}", respons);
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
    static async Task<Results<NotFound, NoContent>> Update(CategoryRequest categoryRequest, IMapper mapper, CategoryService categoryService, IOutputCacheStore outputCacheStore, int id)
    {
        if (!await categoryService.Exist(id))
            return TypedResults.NotFound();
        else
        {
            var request = mapper.Map<Category>(categoryRequest);
            await categoryService.UpdateAsync(request);
            await outputCacheStore.EvictByTagAsync(CacheKey, default);
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
            await outputCacheStore.EvictByTagAsync(CacheKey, default);
            return TypedResults.NoContent();
        }
    }
}

