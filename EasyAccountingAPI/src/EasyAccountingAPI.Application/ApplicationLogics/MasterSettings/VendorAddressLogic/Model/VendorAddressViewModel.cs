namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VendorAddressLogic.Model
{
    public class VendorAddressViewModel
    {
        public VendorAddressCreateModel CreateModel { get; set; }
        public VendorAddressUpdateModel UpdateModel { get; set; }  
        public VendorAddressGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class VendorAddressCreateModel : IMapFrom<VendorAddress>
    {
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Address must be 5 to 500 character.")]
        public string Address { get; set; }

        public string? Fax { get; set; }
        public string? Zip { get; set; }
        public string? Website { get; set; }
        public string? Notes { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VendorAddressCreateModel, VendorAddress>();
        }
    }

    public class VendorAddressUpdateModel : IMapFrom<VendorAddress>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vendor is required.")]
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Address must be 5 to 500 character.")]
        public string Address { get; set; }

        public string? Fax { get; set; }
        public string? Zip { get; set; }
        public string? Website { get; set; }
        public string? Notes { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VendorAddressUpdateModel, VendorAddress>();
            profile.CreateMap<VendorAddress, VendorAddressUpdateModel>();
        }
    }

    public class VendorAddressGridModel : IMapFrom<VendorAddress>
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string? Fax { get; set; }
        public string? Zip { get; set; }
        public string? Website { get; set; }
        public string? Notes { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VendorAddress, VendorAddressGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.CityName, s => s.MapFrom(m => m.City.Name))
                .ForMember(d => d.CountryName, s => s.MapFrom(m => m.Country.Name));
        }
    }
}