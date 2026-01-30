using AutoMapper;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CommentRequestsAndResponses;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CourseRequestsAndResponses;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRAR;
using CourseStoreMinimalAPI.Entities;

namespace CourseStoreMinimalAPI.Endpoint.InfraStructures;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CategoryResponse, Category>().ReverseMap();
        CreateMap<CategoryRequest, Category>().ReverseMap();
        CreateMap<TeacherRequest, Teacher>().ReverseMap();
        CreateMap<TeacherResponse, Teacher>().ReverseMap();
        CreateMap<CourseRequest, Course>().ReverseMap();
        CreateMap<CourseResponse, Course>().ReverseMap();
        CreateMap<CourseWithCommentResponse, Course>().ReverseMap();
        CreateMap<CommentResponse, Comment>().ReverseMap();
        CreateMap<CommentRequest, Comment>().ReverseMap();
    }
}

