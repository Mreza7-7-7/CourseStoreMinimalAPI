using AutoMapper;
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.Endpoint.InfraStructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CommentRequestsAndResponses;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CourseRequestsAndResponses;
using CourseStoreMinimalAPI.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseStoreMinimalAPI.Endpoint.Endpoints;

public static class CommentEndpoint
{
    static string CacheKey = "Comments";
    public static string _prefix;
    public static WebApplication MapComments(this WebApplication app, string prefix)
    {
        _prefix = prefix;
        var MGComments = app.MapGroup(prefix);
        MGComments.MapGet("/{CourseId:int}", Getlist).CacheOutput(c => { c.Expire(TimeSpan.FromMinutes(20)).Tag(CacheKey); });
        MGComments.MapGet("/{id:int}", GetById);
        MGComments.MapGet("/totalCount/{CourseId:int}", TotalCount);
        MGComments.MapPost("/", Insert).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CommentRequest>>();
        MGComments.MapPut("/{id:int}", Update).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CommentRequest>>();
        MGComments.MapDelete("/{id:int}", Delete);
        return app;
    }
    static async Task<Created<CommentResponse>> Insert(CommentService commentService, IOutputCacheStore outputCacheStore, [FromForm] CommentRequest commentRequest, IMapper mapper)
    {
        var comment = mapper.Map<Comment>(commentRequest);

        var savedEntityId = commentService.Insert(comment);
        await outputCacheStore.EvictByTagAsync(CacheKey, default);
        var respons = mapper.Map<CommentResponse>(comment);
        return TypedResults.Created($"/{_prefix}/{savedEntityId}", respons);
    }
    static async Task<Ok<List<CommentResponse>>> Getlist(CommentService commentService, int courseId, IMapper mapper)
    {
        var result = await commentService.GetAll(courseId);
        var response = mapper.Map<List<CommentResponse>>(result);
        return TypedResults.Ok<List<CommentResponse>>(response);
    }
    static async Task<Ok<int>> TotalCount(CommentService commentService)
    {
        int totalCount = await commentService.GetTotalCountAsync();
        return TypedResults.Ok<int>(totalCount);
    }
    static async Task<Results<NotFound, Ok<CommentResponse>>> GetById(CommentService commentService, int id, IMapper mapper)
    {
        var result = await commentService.GetCommentAsync(id);
        var response = mapper.Map<CommentResponse>(result);
        return result == null ? TypedResults.NotFound() : TypedResults.Ok<CommentResponse>(response);
    }
    static async Task<Results<NotFound, NoContent>> Update(CommentService commentService, [FromForm] CommentRequest commentRequest, IMapper mapper, IOutputCacheStore outputCacheStore, int id)
    {
        if (!await commentService.Exist(id))
            return TypedResults.NotFound();

        var CommentForSave = await commentService.GetCommentAsync(id);
        var request = mapper.Map<Comment>(commentRequest);
        CommentForSave.CommentBody = request.CommentBody;
        CommentForSave.CommentDate = request.CommentDate;
        CommentForSave.CourseId = request.CourseId;

        await commentService.Update();
        await outputCacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }
    static async Task<Results<NoContent, NotFound>> Delete(CommentService commentService, IOutputCacheStore outputCacheStore, int id)
    {
        if (!await commentService.Exist(id))
            return TypedResults.NotFound();
        var course = await commentService.GetCommentAsync(id);

        await commentService.Delete(course);
        await outputCacheStore.EvictByTagAsync(CacheKey, default);
        return TypedResults.NoContent();
    }
}

