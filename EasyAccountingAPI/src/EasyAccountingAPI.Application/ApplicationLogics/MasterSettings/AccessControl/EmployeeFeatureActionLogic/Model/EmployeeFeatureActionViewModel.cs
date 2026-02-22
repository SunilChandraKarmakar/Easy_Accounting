namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.EmployeeFeatureActionLogic.Model
{
    public class EmployeeFeatureActionViewModel
    {
        public EmployeeFeatureActionCreateModel CreateModel { get; set; }
        public EmployeeFeatureActionUpdateModel UpdateModel { get; set; }
        public EmployeeFeatureActionGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class EmployeeFeatureActionCreateModel : IMapFrom<EmployeeFeatureAction>
    {
        public int EmployeeId { get; set; }
        public int FeatureId { get; set; }
        public int ActionId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EmployeeFeatureActionCreateModel, EmployeeFeatureAction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }

    public class EmployeeFeatureActionUpdateModel : IMapFrom<EmployeeFeatureAction>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int FeatureId { get; set; }
        public int ActionIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EmployeeFeatureActionUpdateModel, EmployeeFeatureAction>();
            profile.CreateMap<EmployeeFeatureAction, EmployeeFeatureActionUpdateModel>();
        }
    }

    public class EmployeeFeatureActionGridModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<FeatureWithActionsModel> Features { get; set; }
    }

    public class FeatureWithActionsModel
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public List<ActionDetails> Actions { get; set; }
    }

    public class ActionDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}