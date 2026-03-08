namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.ProductLogic.Validator
{
    public class ProductCreateValidator : AbstractValidator<Model.ProductCreateModel>
    {
        public ProductCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(2, 100).WithMessage("Product name must be between 2 and 100 characters.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Product code is required.")
                .Length(6, 40).WithMessage("Product code must be between 6 and 40 characters.");

            RuleFor(x => x.ProductUnitId)
                .GreaterThan(0).WithMessage("Product unit is required.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage("Brand is required.");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Company is required.");

            RuleFor(x => x.CostPrice)
                .GreaterThan(0).WithMessage("Product price must be greater than 0.");

            RuleFor(x => x.SellPrice)
                .GreaterThan(0).WithMessage("Product sell price must be greater than 0.")
                .GreaterThan(x => x.CostPrice).WithMessage("Product sell price must be greater than cost price.");

            When(x => x.HaveProductInventory, () =>
            {
                RuleFor(x => x.ProductInventory).NotNull().WithMessage("Product inventory details are required when HaveProductInventory is true.");
                RuleFor(x => x.ProductInventory).SetValidator(new ProductInventoryLogic.Validator.ProductInventoryCreateValidator());
            });
        }
    }

    public class ProductUpdateValidator : AbstractValidator<Model.ProductUpdateModel>
    {
        public ProductUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid product id.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(2, 100).WithMessage("Product name must be between 2 and 100 characters.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Product code is required.")
                .Length(6, 40).WithMessage("Product code must be between 6 and 40 characters.");

            RuleFor(x => x.ProductUnitId)
                .GreaterThan(0).WithMessage("Product unit is required.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage("Brand is required.");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Company is required.");

            RuleFor(x => x.CostPrice)
                .GreaterThan(0).WithMessage("Product price must be greater than 0.");

            RuleFor(x => x.SellPrice)
                .GreaterThan(0).WithMessage("Product sell price must be greater than 0.")
                .GreaterThan(x => x.CostPrice).WithMessage("Product sell price must be greater than cost price.");

            When(x => x.HaveProductInventory, () =>
            {
                RuleFor(x => x.ProductInventory).NotNull().WithMessage("Product inventory details are required when HaveProductInventory is true.");
                RuleFor(x => x.ProductInventory).SetValidator(new ProductInventoryLogic.Validator.ProductInventoryUpdateValidator());
            });
        }
    }
}