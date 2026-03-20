namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.PurchaseItemLogic.Model
{
    public class PurchaseItemViewModel
    {
        public PurchaseItemCreateModel CreateModel { get; set; }
        public PurchaseItemUpdateModel PurchaseItemUpdateModel { get; set; }
        public PurchaseItemGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class PurchaseItemCreateModel : IMapFrom<PurchaseItem>
    {
        [Required(ErrorMessage = "Please, provide product.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please, provide QTY.")]
        public int Qty { get; set; }

        [Required(ErrorMessage = "Please, provide unit price.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Please, provide sell price.")]
        public decimal SellPrice { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [Required(ErrorMessage = "Please, provide sub total price.")]
        public decimal SubTotal { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PurchaseItemCreateModel, PurchaseItem>();
        }
    }

    public class PurchaseItemUpdateModel : IMapFrom<PurchaseItem>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide product.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please, provide QTY.")]
        public int Qty { get; set; }

        [Required(ErrorMessage = "Please, provide unit price.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Please, provide sell price.")]
        public decimal SellPrice { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [Required(ErrorMessage = "Please, provide sub total price.")]
        public decimal SubTotal { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PurchaseItemUpdateModel, PurchaseItem>();
            profile.CreateMap<PurchaseItem, PurchaseItemUpdateModel>();
        }
    }

    public class PurchaseItemGridModel : IMapFrom<PurchaseItem>
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal SubTotal { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PurchaseItem, PurchaseItemGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.ProductName, s => s.MapFrom(m => m.Product.Name));
        }
    }
}