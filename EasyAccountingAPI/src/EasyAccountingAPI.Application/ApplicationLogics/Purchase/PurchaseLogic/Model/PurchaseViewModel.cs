namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.PurchaseLogic.Model
{
    public class PurchaseViewModel
    {
        public PurchaseCreateModel CreateModel { get; set; }
        public PurchaseUpdateModel UpdateModel { get; set; }
        public PurchaseGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class PurchaseCreateModel : IMapFrom<Purchas>
    {
        [Required(ErrorMessage = "Please, provide order number.")]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Please, provide company.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Please, provide purchase date.")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Please, provide payment date.")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Please, provide vendor.")]
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Please, provide total amount.")]
        public decimal TotalAmount { get; set; }

        public string? Notes { get; set; }

        public ICollection<PurchaseItemCreateModel> PurchaseItems { get; set; } 

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PurchaseCreateModel, Purchas>()
                .ForMember(d => d.PurchaseItems, s => s.MapFrom(m => m.PurchaseItems));
        }
    }

    public class PurchaseUpdateModel : IMapFrom<Purchas>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide order number.")]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Please, provide company.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Please, provide purchase date.")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Please, provide payment date.")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Please, provide vendor.")]
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Please, provide total amount.")]
        public decimal TotalAmount { get; set; }

        public string? Notes { get; set; }

        public ICollection<PurchaseItemUpdateModel> PurchaseItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Purchas, PurchaseUpdateModel>()
                .ForMember(d => d.PurchaseItems, s => s.MapFrom(m => m.PurchaseItems));
            profile.CreateMap<PurchaseUpdateModel, Purchas>()
                .ForMember(d => d.PurchaseItems, s => s.MapFrom(m => m.PurchaseItems));
        }
    }

    public class PurchaseGridModel : IMapFrom<Purchas>
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string CompanyName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string VendorName { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }

        public ICollection<PurchaseItemGridModel> PurchaseItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Purchas, PurchaseGridModel>()
                .ForMember(d => d.PurchaseItems, s => s.MapFrom(m => m.PurchaseItems));
        }
    }
}