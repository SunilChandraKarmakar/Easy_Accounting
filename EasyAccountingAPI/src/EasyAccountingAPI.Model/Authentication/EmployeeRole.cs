namespace EasyAccountingAPI.Model.Authentication
{
    [Table("EmployeeRoles", Schema = "Authentication")]
    public class EmployeeRole
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide employee.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please, provide role.")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Please, provide assignment date.")]
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Please, provide assigned by employee id.")]
        public int AssignedByEmployeeId { get; set; }

        public Employee Employee { get; set; }
        public Role Role { get; set; }
        public Employee AssignedByEmployee { get; set; }
    }
}