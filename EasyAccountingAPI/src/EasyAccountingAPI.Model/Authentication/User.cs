namespace EasyAccountingAPI.Model.Authentication
{
    public class User : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Please, provide full name.")]
        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string FullName { get; set; }
        public int? EmployeeId { get; set; }

        public Employee? Employee { get; set; }
    }
}