namespace EasyAccountingAPI.Model.Authentication
{
    public class Role
    {
        public Role()
        {
            EmployeeRoles = new HashSet<EmployeeRole>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide role name.")]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Please, provide created by employee.")]
        public int CreatedByEmployeeId { get; set; }

        public Employee CreatedByEmployee { get; set; }
        public ICollection<EmployeeRole> EmployeeRoles { get; set; }
    }
}