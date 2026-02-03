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

        [Required(ErrorMessage = "At least one Action is required.")]
        public List<int> ActionIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FeatureActionUpdateModel, FeatureAction>();
            profile.CreateMap<FeatureAction, FeatureActionUpdateModel>();
        }
    }

    public class FeatureActionGridModel : IMapFrom<FeatureAction>
    {
        public int Id { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public List<int> ActionIds { get; set; }
        public List<string> ActionNames { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FeatureAction, FeatureActionGridModel>();
        }
    }
}