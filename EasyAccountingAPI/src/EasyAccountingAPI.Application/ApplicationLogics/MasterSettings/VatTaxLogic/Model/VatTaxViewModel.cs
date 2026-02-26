namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VatTaxLogic.Model
{
    public class VatTaxViewModel
    {
        public VatTaxCreateModel CreateModel { get; set; }
        public VatTaxUpdateModel UpdateModel { get; set; }
        public VatTaxGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class VatTaxCreateModel : IMapFrom<VatTax>
    {
        [Required(ErrorMessage = "Tax Name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string TaxName { get; set; }

        [Required(ErrorMessage = "Tax Rate is required.")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public string? TaxNumber { get; set; }
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VatTaxCreateModel, VatTax>();
        }
    }

    public class VatTaxUpdateModel : IMapFrom<VatTax>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tax Name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string TaxName { get; set; }

        [Required(ErrorMessage = "Tax Rate is required.")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public string? TaxNumber { get; set; }
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VatTax, VatTaxUpdateModel>();
            profile.CreateMap<VatTaxUpdateModel, VatTax>();
        }
    }

    public class VatTaxGridModel : IMapFrom<VatTax>
    {
        public string Id { get; set; }
        public string TaxName { get; set; }
        public decimal Rate { get; set; }
        public string CompanyName { get; set; }
        public string? TaxNumber { get; set; }
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VatTax, VatTaxGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company.Name));
        }
    }
}