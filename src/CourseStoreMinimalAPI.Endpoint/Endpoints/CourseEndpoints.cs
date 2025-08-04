using AutoMapper;
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.Endpoint.InfraStructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CourseRequestsAndResponses;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRAR;
using CourseStoreMinimalAPI.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseStoreMinimalAPI.Endpoint.Endpoints;

public static class CourseEndpoints
{
    static string CacheKey = "Courses";
    static string CourseImageFolder = @"Images\Courses";
    static string DefaultCourseImageName = "Default.jpg";
    public static string _prefix;
    public static WebApplication MapCourses(this WebApplication app, string prefix)
    {
        _prefix = prefix;
        var MGTeachers = app.MapGroup(prefix);
        MGTeachers.MapGet("/{pageNumber}/{countInPage}", Getlist).CacheOutput(c => { c.Expire(TimeSpan.FromMinutes(20)).Tag(CacheKey); });
        MGTeachers.MapGet("/{id:int}", GetById);
        MGTeachers.MapGet("/search", Search);
        MGTeachers.MapGet("/totalCount", TotalCount);
        MGTeachers.MapPost("/", Insert).DisableAntiforgery();
        MGTeachers.MapPut("/{id:int}", Update).DisableAntiforgery();
        MGTeachers.MapDelete("/{id:int}", Delete);
        return app;
    }
    static async Task<Created<CourseResponse>> Insert(CourseService courseService, IFileAdapter fileAdapter, IOutputCacheStore outputCacheStore, [FromForm] CourseRequest courseRequest, IMapper mapper)
    {
        var course = mapper.Map<Course>(courseRequest);
        string fileName = DefaultCourseImageName;
        if (courseRequest.File is not null)
        {
            fileName = fileAdapter.InsertFile(courseRequest.File, CourseImageFolder);
        }
        course.ImageUrl = fileName;
        var savedEntityId = courseService.Insert(course);
        await outputCacheStore.EvictByTagAsync(CacheKey, default);
        var respons = mapper.Map<CourseResponse>(course);
        return TypedResults.Created($"/{_prefix}/{savedEntityId}", respons);
    }
    static async Task<Ok<List<CourseResponse>>> Getlist(CourseService courseService, int pageNumber, int countInPage, IMapper mapper)
    {
        var result = await courseService.GetAll(pageNumber, countInPage);
        var response = mapper.Map<List<CourseResponse>>(result);
        return TypedResults.Ok<List<CourseResponse>>(response);
    }
    static async Task<Ok<int>> TotalCount(CourseService courseService)
    {
        int totalCount = await courseService.GetTotalCountAsync();
        return TypedResults.Ok<int>(totalCount);
    }
    static async Task<Results<NotFound, Ok<CourseResponse>>> GetById(CourseService courseService, int id, IMapper mapper)
    {
        var result = await courseService.GetCourseAsync(id);
        var response = mapper.Map<CourseResponse>(result);
        return result == null ? TypedResults.NotFound() : TypedResults.Ok<CourseResponse>(response);
    }
    static async Task<Results<NotFound, Ok<List<CourseResponse>>>> Search(CourseService courseService, string? title, bool? isOnline, IMapper mapper)
    {
        var result = await courseService.Search(title, isOnline);
        var response = mapper.Map<List<CourseResponse>>(result);
        return result == null ? TypedResults.NotFound() : TypedResults.Ok<List<CourseResponse>>(response);
    }
    static async Task<Results<NotFound, NoContent>> Update(CourseService courseService, [FromForm] CourseRequest courseRequest, IMapper mapper, IFileAdapter fileAdapter, IOutputCacheStore outputCacheStore, int id)
    {
        if (!await courseService.Exist(id))
            return TypedResults.NotFound();

        var courseForSave = await courseService.GetCourseAsync(id);
        var request = mapper.Map<Course>(courseRequest);
        courseForSave.Title = request.Title;
        courseForSave.Description = request.Description;
        courseForSave.StartDate = request.StartDate;
        courseForSave.EndDate = request.EndDate;
        courseForSave.IsOnline = request.IsOnline;

        if (courseRequest.File is not null)
        {
            courseForSave.ImageUrl = fileAdapter.Update(courseForSave.ImageUrl, courseRequest.File, CourseImageFolder);
        }
        await courseService.Update();
        await outputCacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }
    static async Task<Results<NoContent, NotFound>> Delete(CourseService courseService, IFileAdapter fileAdapter, IOutputCacheStore outputCacheStore, int id)
    {
        if (!await courseService.Exist(id))
            return TypedResults.NotFound();
        var course = await courseService.GetCourseAsync(id);
        fileAdapter.DeleteFile(course.ImageUrl, CourseImageFolder);
        await courseService.Delete(course);
        await outputCacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }
}

