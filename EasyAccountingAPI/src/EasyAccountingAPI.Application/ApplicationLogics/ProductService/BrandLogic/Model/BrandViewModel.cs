namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.BrandLogic.Model
{
    public class BrandViewModel
    {
        public BrandCreateModel CreateModel { get; set; }
        public BrandUpdateModel UpdateModel { get; set; }   
        public BrandGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class BrandCreateModel : IMapFrom<Brand>
    {
        [Required(ErrorMessage = "Brand name is required.")]
        [StringLength(100, ErrorMessage = "Brand name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BrandCreateModel, Brand>();
        }
    }

    public class BrandUpdateModel : IMapFrom<Brand>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Brand name is required.")]
        [StringLength(100, ErrorMessage = "Brand name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Brand, BrandUpdateModel>();
            profile.CreateMap<BrandUpdateModel, Brand>();
        }
    }

    public class BrandGridModel : IMapFrom<Brand>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Brand, BrandGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company.Name));
        }
    }
}