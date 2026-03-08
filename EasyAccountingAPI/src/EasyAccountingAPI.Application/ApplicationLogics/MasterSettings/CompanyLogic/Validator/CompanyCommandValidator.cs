namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CompanyLogic.Validator
{
    public class CompanyCreateCommandValidator : AbstractValidator<CompanyCreateCommand>
    {
        public CompanyCreateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name is required.")
                .Length(2, 100).WithMessage("Company name must be between 2 and 100 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Length(11, 30).WithMessage("Phone number must be between 11 and 30 characters.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("Please, provide country.");

            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage("Please, provide city.");

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0).WithMessage("Please, provide currency.");
        }
    }

    public class CompanyUpdateCommandValidator : AbstractValidator<CompanyUpdateCommand>
    {
        public CompanyUpdateCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid company id.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name is required.")
                .Length(2, 100).WithMessage("Company name must be between 2 and 100 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Length(11, 30).WithMessage("Phone number must be between 11 and 30 characters.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("Please, provide country.");

            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage("Please, provide city.");

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0).WithMessage("Please, provide currency.");
        }
    }
}