namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VatTaxLogic.Validator
{
    public class VatTaxCreateValidator : AbstractValidator<Model.VatTaxCreateModel>
    {
        public VatTaxCreateValidator()
        {
            RuleFor(x => x.TaxName)
                .NotEmpty().WithMessage("Tax Name is required.")
                .MinimumLength(2).WithMessage("Tax Name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Tax Name cannot exceed 100 characters.");

            RuleFor(x => x.Rate)
                .GreaterThan(0).WithMessage("Tax Rate must be greater than 0.");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Company is required.");
        }
    }

    public class VatTaxUpdateValidator : AbstractValidator<Model.VatTaxUpdateModel>
    {
        public VatTaxUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid tax id.");

            RuleFor(x => x.TaxName)
                .NotEmpty().WithMessage("Tax Name is required.")
                .MinimumLength(2).WithMessage("Tax Name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Tax Name cannot exceed 100 characters.");

            RuleFor(x => x.Rate)
                .GreaterThan(0).WithMessage("Tax Rate must be greater than 0.");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Company is required.");
        }
    }
}