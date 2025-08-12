using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRAR;
using FluentValidation;
namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.TeacherRequestAndResponses;

public class TeacherRequestValidator : AbstractValidator<TeacherRequest>
{
    public TeacherRequestValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().WithMessage("{PropertyName} Could Not Be Empty")
            .MaximumLength(50);
    }
}

