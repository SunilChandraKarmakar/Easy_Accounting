namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Validator
{
    public class CurrencyCreateValidator : AbstractValidator<CurrencyCreateModel>
    {
        public CurrencyCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Currency name is required.")
                .Length(2, 50).WithMessage("Currency name must be between 2 and 50 characters.");

            RuleFor(x => x.BaseRate)
                .GreaterThan(0).WithMessage("Base rate must be greater than 0.");
        }
    }

    public class CurrencyUpdateValidator : AbstractValidator<CurrencyUpdateModel>
    {
        public CurrencyUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid currency id.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Currency name is required.")
                .Length(2, 50).WithMessage("Currency name must be between 2 and 50 characters.");

            RuleFor(x => x.BaseRate)
                .GreaterThan(0).WithMessage("Base rate must be greater than 0.");
        }
    }
}