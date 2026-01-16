namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CompanyLogic.Model
{
    public class CompanyViewModel
    {
        public CompanyCreateModel CreateModel { get; set; }
        public CompanyUpdateModel UpdateModel { get; set; }
        public CompanyGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class CompanyCreateModel : IMapFrom<Company>
    {
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        public string? Email { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(30, MinimumLength = 11)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please, provide country.")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Please, provide city.")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Please, provide currency.")]
        public int CurrencyId { get; set; }

        public string? Logo { get; set; }
        public string? TaxNo { get; set; }
        public bool IsSellWithPos { get; set; }
        public bool IsProductHaveBrand { get; set; }
        public bool IsDefaultCompany { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? Address { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CompanyCreateModel, Company>();
            profile.CreateMap<Company, CompanyCreateModel>();
        }
    }

    public class CompanyUpdateModel : IMapFrom<Company>
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        public string? Email { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(30, MinimumLength = 11)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please, provide country.")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Please, provide city.")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Please, provide currency.")]
        public int CurrencyId { get; set; }

        public string? Logo { get; set; }
        public string? TaxNo { get; set; }
        public bool IsSellWithPos { get; set; }
        public bool IsProductHaveBrand { get; set; }
        public bool IsDefaultCompany { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? Address { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CompanyUpdateModel, Company>();
            profile.CreateMap<Company, CompanyUpdateModel>();
        }
    }

    public class CompanyGridModel : IMapFrom<Company>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string CurrencyName { get; set; }
        public string? Logo { get; set; }
        public string? TaxNo { get; set; }
        public bool IsSellWithPos { get; set; }
        public bool IsProductHaveBrand { get; set; }
        public bool IsDefaultCompany { get; set; }
        public string? Address { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Company, CompanyGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.CountryName, s => s.MapFrom(m => m.Country.Name))
                .ForMember(d => d.CityName, s => s.MapFrom(m => m.City.Name))
                .ForMember(d => d.CurrencyName, s => s.MapFrom(m => m.Currency.Name));
        }
    }
}