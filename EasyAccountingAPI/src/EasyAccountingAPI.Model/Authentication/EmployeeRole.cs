namespace EasyAccountingAPI.Model.Authentication
{
    public class EmployeeRole
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide employee.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please, provide role.")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Please, provide assignment date.")]
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        public int? AssignedByEmployeeId { get; set; }

        public Employee Employee { get; set; }
        public Role Role { get; set; }
    }
}