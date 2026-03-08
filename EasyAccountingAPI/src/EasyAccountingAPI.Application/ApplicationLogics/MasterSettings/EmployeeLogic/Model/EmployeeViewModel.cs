namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.EmployeeLogic.Model
{
    public class EmployeeViewModel
    {
        public EmployeeCreateModel CreateModel { get; set; }
        public EmployeeUpdateModel UpdateModel { get; set; }    
        public EmployeeGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class EmployeeCreateModel : IMapFrom<Employee>
    {
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
        public int? CompanyId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EmployeeCreateModel, Employee>();
        }
    }

    public class EmployeeUpdateModel : IMapFrom<Employee>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string? Image { get; set; }
        public int? CompanyId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Employee, EmployeeUpdateModel>();
            profile.CreateMap<EmployeeUpdateModel, Employee>();
        }
    }

    public class EmployeeGridModel : IMapFrom<Employee>
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string? Image { get; set; }
        public string? CompanyName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Employee, EmployeeGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company.Name));
        }
    }
}