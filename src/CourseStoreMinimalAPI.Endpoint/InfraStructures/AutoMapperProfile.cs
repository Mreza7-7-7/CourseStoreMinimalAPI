using AutoMapper;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using CourseStoreMinimalAPI.Entities;

namespace CourseStoreMinimalAPI.Endpoint.InfraStructures
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CategoryRespons, Category>().ReverseMap();
            CreateMap<CategoryRequest, Category>().ReverseMap();
        }
    }
}
