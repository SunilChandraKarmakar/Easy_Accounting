namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureActionLogic.Model
{
    public class FeatureActionViewModel
    {
        public FeatureActionCreateModel CreateModel { get; set; }
        public FeatureActionUpdateModel UpdateModel { get; set; }
        public FeatureActionGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class FeatureActionCreateModel : IMapFrom<FeatureAction>
    {
        [NotMapped] public int ModuleId { get; set; }

        [Required(ErrorMessage = "Feature is required.")]
        public int FeatureId { get; set; }

        [Required(ErrorMessage = "At least one Action is required.")]
        public List<int> ActionIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FeatureActionCreateModel, FeatureAction>();
        }
    }

    public class FeatureActionUpdateModel : IMapFrom<FeatureAction>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Feature is required.")]
        public int FeatureId { get; set; }

        [NotMapped] public string FeatureName { get; set; }

        [Required(ErrorMessage = "At least one Action is required.")]
        public List<int> ActionIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FeatureActionUpdateModel, FeatureAction>();
            profile.CreateMap<FeatureAction, FeatureActionUpdateModel>()
                .ForMember(d => d.FeatureName, s => s.MapFrom(m => m.Feature.Name));
        }
    }

    public class FeatureActionGridModel
    {
        public int FeatureId { get; set; }
        public string? EncriptedFeatureId { get; set; }
        public string FeatureName { get; set; }
        public List<FeatureActionStatusModel> Actions { get; set; }
    }

    public class FeatureActionStatusModel
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public bool IsEnabled { get; set; }
    }
}