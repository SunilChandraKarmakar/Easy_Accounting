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
        public string Name { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int CurrencyId { get; set; }
        public IFormFile? LogoFile { get; set; }
        public string? TaxNo { get; set; }
        public bool IsSellWithPos { get; set; }
        public bool IsProductHaveBrand { get; set; } 
        public bool IsDefaultCompany { get; set; }
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
        public string Name { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int CurrencyId { get; set; }
        public string? Logo { get; set; }
        public IFormFile? LogoFile { get; set; }
        public bool IsRemoveLogo { get; set; }
        public string? TaxNo { get; set; }
        public bool IsSellWithPos { get; set; }
        public bool IsProductHaveBrand { get; set; }
        public bool IsDefaultCompany { get; set; }
        public string? Address { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CompanyUpdateModel, Company>()
             .ForMember(d => d.Logo, s => s.Ignore()); 

            profile.CreateMap<Company, CompanyUpdateModel>()
                .ForMember(d => d.LogoFile, s => s.Ignore());
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