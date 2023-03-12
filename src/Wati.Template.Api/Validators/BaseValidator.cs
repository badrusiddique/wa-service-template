using FluentValidation;

namespace Wati.Template.Api.Validators;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    protected bool BeAValidDate(DateTime date) => !date.Equals(default);

    protected bool BeAValidDate(DateTime? date) => !(date.HasValue && date.Equals(default));
}