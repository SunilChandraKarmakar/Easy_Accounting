namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.ProductInventoryLogic.Validator
{
    public class ProductInventoryCreateValidator : AbstractValidator<Model.ProductInventoryCreateModel>
    {
        public ProductInventoryCreateValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Product is required.");
            RuleFor(x => x.OpeningStock).GreaterThan(0).WithMessage("Opening stock must be greater than 0.");

            When(x => x.HaveStockAlert, () =>
            {
                RuleFor(x => x.StockAlertQty)
                    .NotNull().WithMessage("Stock alert quantity is required when stock alert is enabled.")
                    .GreaterThan(0).WithMessage("Stock alert quantity must be greater than 0 when stock alert is enabled.");
            });
        }
    }

    public class ProductInventoryUpdateValidator : AbstractValidator<Model.ProductInventoryUpdateModel>
    {
        public ProductInventoryUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid inventory id.");
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Product is required.");
            RuleFor(x => x.OpeningStock).GreaterThan(0).WithMessage("Opening stock must be greater than 0.");

            When(x => x.HaveStockAlert, () =>
            {
                RuleFor(x => x.StockAlertQty)
                    .NotNull().WithMessage("Stock alert quantity is required when stock alert is enabled.")
                    .GreaterThan(0).WithMessage("Stock alert quantity must be greater than 0 when stock alert is enabled.");
            });
        }
    }
}