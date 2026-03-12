namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VendorLogic.Model
{
    public class VendorViewModel
    {
        public VendorCreateModel CreateModel { get; set; }
        public VendorUpdateModel UpdateModel { get; set; }
        public VendorGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class VendorCreateModel : IMapFrom<Vendor>
    {
        [Required(ErrorMessage = "Business Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Business name must be 2 to 50 character.")]
        public string BusinessName { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be 2 to 100 character.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [StringLength(100, MinimumLength = 11, ErrorMessage = "Email must be 11 to 100 character.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public string? Phone { get; set; }
        public string? Image { get; set; }

        public VendorAddressCreateModel VendorAddress { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VendorCreateModel, Vendor>()
                 .ForMember(d => d.VendorAddresses, s => s.MapFrom(m => m.VendorAddress != null ? new[] { m.VendorAddress } : Enumerable.Empty<VendorAddressCreateModel>()));
        }
    }

    public class VendorUpdateModel : IMapFrom<Vendor>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Business Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Business name must be 2 to 50 character.")]
        public string BusinessName { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be 2 to 100 character.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [StringLength(100, MinimumLength = 11, ErrorMessage = "Email must be 11 to 100 character.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public string? Phone { get; set; }
        public string? Image { get; set; }

        public VendorAddressUpdateModel VendorAddress { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VendorUpdateModel, Vendor>()
                 .ForMember(d => d.VendorAddresses, s => s.MapFrom(m => m.VendorAddress != null ? new[] { m.VendorAddress } : Enumerable.Empty<VendorAddressUpdateModel>()));
            profile.CreateMap<Vendor, VendorUpdateModel>()
                .ForMember(d => d.VendorAddress, s => s.MapFrom(m => m.VendorAddresses != null ? m.VendorAddresses.FirstOrDefault() : null))
                .AfterMap((s, d) => { if (d.VendorAddress == null) d.VendorAddress = new VendorAddressUpdateModel(); });
        }
    }

    public class VendorGridModel : IMapFrom<Vendor>
    {
        public string Id { get; set; }
        public string BusinessName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Image { get; set; }
        public string CompanyName { get; set; }

        public VendorAddressGridModel VendorAddress { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Vendor, VendorGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company))
                .ForMember(d => d.VendorAddress, s => s.MapFrom(m => m.VendorAddresses != null ? m.VendorAddresses.FirstOrDefault() : null));
        }
    }
}