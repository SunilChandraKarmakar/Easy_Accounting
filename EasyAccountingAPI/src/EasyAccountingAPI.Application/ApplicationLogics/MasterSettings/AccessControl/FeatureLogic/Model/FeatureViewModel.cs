namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureLogic.Model
{
    public class FeatureViewModel
    {
        public FeatureCreateModel CreateModel { get; set; }
        public FeatureUpdateModel UpdateModel { get; set; }
        public FeatureGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class FeatureCreateModel : IMapFrom<Feature>
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        [StringLength(20, MinimumLength = 2)]
        public string Code { get; set; }

        [Required(ErrorMessage = "Module is required.")]
        public int ModuleId { get; set; }

        public string? ControllerName { get; set; }
        public string? TableName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FeatureCreateModel, Feature>();
            profile.CreateMap<Feature, FeatureCreateModel>();
        }
    }

    public class FeatureUpdateModel : IMapFrom<Feature>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        [StringLength(20, MinimumLength = 2)]
        public string Code { get; set; }

        [Required(ErrorMessage = "Module is required.")]
        public int ModuleId { get; set; }

        public string? ControllerName { get; set; }
        public string? TableName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FeatureUpdateModel, Feature>();
            profile.CreateMap<Feature, FeatureUpdateModel>();
        }
    }

    public class FeatureGridModel : IMapFrom<Feature>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ModuleName { get; set; }
        public string? ControllerName { get; set; }
        public string? TableName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Feature, FeatureGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.ModuleName, s => s.MapFrom(m => m.Module.Name));
        }
    }
}