using AutoMapper;
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.Endpoint.InfraStructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRAR;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRequestAndResponses;

public class TeacherParameters
{
    public TeacherService TeacherService { get; set; }
    public IFileAdapter FileAdapter { get; set; }
    public IOutputCacheStore OutputCacheStore { get; set; }
    public IMapper Mapper { get; set; }
}

