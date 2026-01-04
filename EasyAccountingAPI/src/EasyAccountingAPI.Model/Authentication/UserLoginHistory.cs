namespace EasyAccountingAPI.Model.Authentication
{
    public class UserLoginHistory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide user id.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Please, provide login IP.")]
        public string LoginIp { get; set; }

        [Required(ErrorMessage = "Please, provide login date & time.")]
        public DateTime? LoginDateTime { get; set; }
        public DateTime? LogoutDateTime { get; set; }
    }
}