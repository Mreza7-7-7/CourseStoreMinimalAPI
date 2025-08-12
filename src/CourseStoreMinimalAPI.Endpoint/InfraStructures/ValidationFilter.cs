
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using FluentValidation;

namespace CourseStoreMinimalAPI.Endpoint.InfraStructures;

public class ValidationFilter<Tmodel> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<Tmodel>>();
        if (validator == null)
        {
            return await next(context);
        }
        var modelForValidate = context.Arguments.OfType<Tmodel>().FirstOrDefault();
        if (modelForValidate == null)
        {
            return Results.Problem("input model is invalid");
        }
        var validationResult = await validator.ValidateAsync(modelForValidate);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var result = await next(context);

        return result;
    }
}

