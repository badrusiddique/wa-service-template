using FluentValidation;
using Wati.Template.Common.Dtos.Request;

namespace Wati.Template.Api.Validators;

public class DomainQueryRequestDtoValidator : BaseValidator<DomainQueryModel>
{
    public DomainQueryRequestDtoValidator()
    {
        RuleFor(p => p.StartDate)
            .Cascade(CascadeMode.Stop)
            .Must(BeAValidDate).WithMessage("Invalid start date, it should be a date type")
            .LessThanOrEqualTo(x => DateTime.UtcNow).WithMessage("Invalid start date, it should be less than or equal to today's date");


        RuleFor(p => p.EndDate)
            .Cascade(CascadeMode.Stop)
            .Must(BeAValidDate).WithMessage("Invalid end date, it should be a date type")
            .GreaterThanOrEqualTo(x => x.StartDate ?? DateTime.MinValue).WithMessage("Invalid end date, it should be greater than or equal to start date");
    }
}