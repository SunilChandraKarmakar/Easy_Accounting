namespace EasyAccountingAPI.Model.Authentication
{
    [Table("Employees", Schema = "Authentication")]
    public class Employee : IAuditableEntity, IDelatableEntity
    {
        public Employee()
        {
            EmployeeRoles = new HashSet<EmployeeRole>();
            AssignedByEmployeeRoles = new HashSet<EmployeeRole>();
            EmployeeFeatureActions = new HashSet<EmployeeFeatureAction>();
        }

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
        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Company? Company { get; set; }
        public User? User { get; set; }
        public ICollection<EmployeeRole> EmployeeRoles { get; set; }
        public ICollection<EmployeeRole> AssignedByEmployeeRoles { get; set; }
        public ICollection<EmployeeFeatureAction> EmployeeFeatureActions { get; set; }
    }
}