using AutoMapper;
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.Endpoint.InfraStructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRAR;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRequestAndResponses;
using CourseStoreMinimalAPI.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.OpenApi.Validations;

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
        MGTeachers.MapGet("/{pageNumber}/{countInPage}", Getlist).CacheOutput(c => { c.Expire(TimeSpan.FromMinutes(20)).Tag(CacheKey); });
        MGTeachers.MapGet("/{id:int}", GetById);
        MGTeachers.MapGet("/search", Search);
        MGTeachers.MapGet("/totalCount", TotalCount);
        MGTeachers.MapPost("/", Insert).DisableAntiforgery().AddEndpointFilter<ValidationFilter<TeacherRequest>>();
        MGTeachers.MapPut("/{id:int}", Update).DisableAntiforgery().AddEndpointFilter<ValidationFilter<TeacherRequest>>();
        MGTeachers.MapDelete("/{id:int}", Delete);
        return app;
    }
    static async Task<Results<Created<TeacherResponse>, ValidationProblem>> Insert([AsParameters] TeacherParameters parameters, [FromForm] TeacherRequest teacherRequest)
    {
        var teacher = parameters.Mapper.Map<Teacher>(teacherRequest);
        string fileName = DefaultTeacherImageName;
        if (teacherRequest.File is not null)
        {
            fileName = parameters.FileAdapter.InsertFile(teacherRequest.File, TeacherImageFolder);
        }
        teacher.ImageUrl = fileName;
        var savedEntityId = parameters.TeacherService.Insert(teacher);
        await parameters.OutputCacheStore.EvictByTagAsync(CacheKey, default);
        var respons = parameters.Mapper.Map<TeacherResponse>(teacher);
        return TypedResults.Created($"/{_prefix}/{savedEntityId}", respons);
    }
    static async Task<Ok<List<TeacherResponse>>> Getlist(TeacherService teacherService, int pageNumber, int countInPage, IMapper mapper)
    {
        var result = await teacherService.GetAll(pageNumber, countInPage);
        var response = mapper.Map<List<TeacherResponse>>(result);
        return TypedResults.Ok<List<TeacherResponse>>(response);
    }
    static async Task<Ok<int>> TotalCount(TeacherService teacherService)
    {
        int totalCount = await teacherService.GetTotalCountAsync();
        return TypedResults.Ok<int>(totalCount);
    }
    static async Task<Results<NotFound, Ok<TeacherResponse>>> GetById(TeacherService teacherService, int id, IMapper mapper)
    {
        var result = await teacherService.GetTeacherAsync(id);
        var response = mapper.Map<TeacherResponse>(result);
        return result == null ? TypedResults.NotFound() : TypedResults.Ok<TeacherResponse>(response);
    }
    static async Task<Results<NotFound, Ok<List<TeacherResponse>>>> Search(TeacherService teacherService, string? firstName, string? lastName, IMapper mapper)
    {
        var result = await teacherService.Search(firstName, lastName);
        var response = mapper.Map<List<TeacherResponse>>(result);
        return result == null ? TypedResults.NotFound() : TypedResults.Ok<List<TeacherResponse>>(response);
    }
    static async Task<Results<NotFound, NoContent>> Update([FromForm] TeacherRequest teacherRequest, IMapper mapper, TeacherService teacherService, IFileAdapter fileAdapter, IOutputCacheStore outputCacheStore, int id)
    {
        if (!await teacherService.Exist(id))
            return TypedResults.NotFound();

        var teacherForSave = await teacherService.GetTeacherAsync(id);
        var request = mapper.Map<Teacher>(teacherRequest);
        teacherForSave.FirstName = request.FirstName;
        teacherForSave.LastName = request.LastName;
        teacherForSave.Birthdate = request.Birthdate;

        if (teacherRequest.File is not null)
        {
            teacherForSave.ImageUrl = fileAdapter.Update(teacherForSave.ImageUrl, teacherRequest.File, TeacherImageFolder);
        }
        await teacherService.Update();
        await outputCacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }
    static async Task<Results<NoContent, NotFound>> Delete(TeacherService teacherService, IFileAdapter fileAdapter, IOutputCacheStore outputCacheStore, int id)
    {
        if (!await teacherService.Exist(id))
            return TypedResults.NotFound();
        var teacher = await teacherService.GetTeacherAsync(id);
        fileAdapter.DeleteFile(teacher.ImageUrl, TeacherImageFolder);
        await teacherService.Delete(teacher);
        await outputCacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();

    }
}

