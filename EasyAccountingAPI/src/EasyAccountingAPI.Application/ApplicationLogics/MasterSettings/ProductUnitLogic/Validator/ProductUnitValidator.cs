namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ProductUnitLogic.Validator
{
    public class ProductUnitCreateValidator : AbstractValidator<Model.ProductUnitCreateModel>
    {
        public ProductUnitCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Unit name is required.")
                .MaximumLength(50).WithMessage("Unit name cannot exceed 50 characters.");
        }
    }

    public class ProductUnitUpdateValidator : AbstractValidator<Model.ProductUnitUpdateModel>
    {
        public ProductUnitUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid unit id.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Unit name is required.")
                .MaximumLength(50).WithMessage("Unit name cannot exceed 50 characters.");
        }
    }
}