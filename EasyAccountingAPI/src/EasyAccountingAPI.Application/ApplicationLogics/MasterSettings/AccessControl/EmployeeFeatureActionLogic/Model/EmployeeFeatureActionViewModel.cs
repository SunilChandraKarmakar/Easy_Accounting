namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.EmployeeFeatureActionLogic.Model
{
    public class EmployeeFeatureActionViewModel
    {
        public EmployeeFeatureActionCreateModel CreateModel { get; set; }
        public ICollection<EmployeeFeatureActionUpdateModel> UpdateModel { get; set; }
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
        [NotMapped] public string? EmployeeName { get; set; }
        [NotMapped] public string? CompanyName { get; set; }
        [NotMapped] public int? ModuleId { get; set; }
        public int FeatureId { get; set; }
        public int ActionId { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<EmployeeFeatureActionUpdateModel, EmployeeFeatureAction>();
            profile.CreateMap<EmployeeFeatureAction, EmployeeFeatureActionUpdateModel>()
                .ForMember(d => d.EmployeeName, s => s.MapFrom(m => m.Employee.FullName))
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Employee.Company == null ? string.Empty : m.Employee.Company.Name))
                .ForMember(d => d.ModuleId, s => s.MapFrom(m => m.Feature.ModuleId));
        }
    }

    public class EmployeeFeatureActionGridModel
    {
        [NotMapped] public string EmployeeEncryptedId { get; set; }
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

    public class EmployeeProjection
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}