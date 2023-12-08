using FluentValidation;
using MoneyLoaner.Data.DTOs;

namespace MoneyLoaner.Data.FluentValidator;

public class ProposalModelFluentValidator : AbstractValidator<ProposalDto>
{
    public ProposalModelFluentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Pole nie może być puste...")
            .Length(1, 100)
            .WithMessage("Zbyt długa nazwa...");

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage("Pole nie może być puste...")
            .Length(1, 100)
            .WithMessage("Zbyt długa nazwa...");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Pole nie może być puste...");

        RuleFor(x => x.PersonalNumber)
            .NotEmpty()
            .WithMessage("Pole nie może być puste...");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Pole nie może być puste...")
            .EmailAddress()
            .WithMessage("Nie poprawny adres email...")
            .Length(1, 100)
            .WithMessage("Zbyt długi adres...");

        RuleFor(x => x.MonthlyIncome)
            .Must(x => x.HasValue)
            .WithMessage("Proszę podać kwotę...")
            .GreaterThan(0)
            .WithMessage("Kwota musi być większa niż 0 zł...");

        RuleFor(x => x.MonthlyExpenses)
            .Must(x => x.HasValue)
            .WithMessage("Proszę podać kwotę...")
            .GreaterThan(0)
            .WithMessage("Kwota musi być większa niż 0 zł...");

        RuleFor(x => x.CCNumber)
            .NotEmpty()
            .WithMessage("Pole nie może być puste...")
            .Length(32, 32)
            .WithMessage("Podaj numer 26 cyfr...");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValues => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<ProposalDto>
            .CreateWithOptions((ProposalDto)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();

        return result.Errors.Select(e => e.ErrorMessage);
    };

    //public Func<object, string, Task<IEnumerable<string>>> ValidateValues2 => async (model, propertyName) =>
    //{
    //    var result = await ValidateAsync(ValidationContext<ProposalDto>
    //        .CreateWithOptions((ProposalDto)model, x => x.UseCustomSelector(selector: FluentValidation.));

    //    if (result.IsValid)
    //        return Array.Empty<string>();

    //    return result.Errors.Select(e => e.ErrorMessage);
    //};
}