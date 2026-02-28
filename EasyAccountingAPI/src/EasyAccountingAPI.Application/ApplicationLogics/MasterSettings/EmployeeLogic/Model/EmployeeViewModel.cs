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
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Please, provide full name.")]
        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please, provide email address.")]
        [EmailAddress]
        [StringLength(maximumLength: 50, MinimumLength = 10)]
        public string Email { get; set; }

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

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Please, provide full name.")]
        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please, provide email address.")]
        [EmailAddress]
        [StringLength(maximumLength: 50, MinimumLength = 10)]
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
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company.Name));
        }
    }
}