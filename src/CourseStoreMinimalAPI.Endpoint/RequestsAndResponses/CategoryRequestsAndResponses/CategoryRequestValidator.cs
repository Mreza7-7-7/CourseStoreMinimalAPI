using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using FluentValidation;
namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRequestsAndResponses;


public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator(CategoryService categoryService, IHttpContextAccessor httpContextAccessor)
    {
        int id = 0;
        if (httpContextAccessor?.HttpContext?.Request.RouteValues.ContainsKey("id") == true)
        {
            var routId = httpContextAccessor.HttpContext.Request.RouteValues["id"];
            id = int.Parse(routId.ToString());
        }
        RuleFor(c => c.Name).NotEmpty().WithMessage(ValidationMessages.REQUIERD).WithName(PropertyName.Name)
       .MustAsync(async (c, CancellationToken) =>
       {
           return await categoryService.IsUnique(id, c);
       });
    }
}

