namespace EasyAccountingAPI.Model.Authentication
{
    public class Employee
    {
        public Employee()
        {
            Roles = new HashSet<Role>();
            EmployeeRoles = new HashSet<EmployeeRole>();
        }

        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Please, provide full name.")]
        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string FullName { get; set; }

        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please, provide email address.")]
        [EmailAddress]
        [StringLength(maximumLength: 50, MinimumLength = 10)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, provide password.")]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Password { get; set; }

        public string? Image { get; set; }

        public User? User { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<EmployeeRole> EmployeeRoles { get; set; }
    }
}