using AutoMapper;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using CourseStoreMinimalAPI.Entities;

namespace CourseStoreMinimalAPI.Endpoint.InfraStructures
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CategoryResponse, Category>().ReverseMap();
            CreateMap<CategoryRequest, Category>().ReverseMap();
        }
    }
}
