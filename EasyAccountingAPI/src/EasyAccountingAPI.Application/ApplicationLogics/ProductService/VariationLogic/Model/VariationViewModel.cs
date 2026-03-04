namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.VariationLogic.Model
{
    public class VariationViewModel
    {
        public VariationCreateModel CreateModel { get; set; }
        public VariationUpdateModel UpdateModel { get; set; }
        public VariationGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class VariationCreateModel : IMapFrom<Variation>
    {
        [Required(ErrorMessage = "Variation name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Variation name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Variation values are required.")]
        public string[] Values { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VariationCreateModel, Variation>();
        }
    }

    public class VariationUpdateModel : IMapFrom<Variation>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Variation name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Variation name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Variation values are required.")]
        public string[] Values { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Variation, VariationUpdateModel>();
            profile.CreateMap<VariationUpdateModel, Variation>();
        }
    }

    public class VariationGridModel : IMapFrom<Variation>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string[] Values { get; set; }
        public string CompanyName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Variation, VariationGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company.Name));
        }
    }
}