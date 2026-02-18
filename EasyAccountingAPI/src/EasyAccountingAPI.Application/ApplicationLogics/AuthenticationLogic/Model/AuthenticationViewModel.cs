namespace EasyAccountingAPI.Application.ApplicationLogics.AuthenticationLogic.Model
{
    public class AuthenticationViewModel
    {
        public UserModel UserModel { get; set; }
        public RegisterModel RegisterModel { get; set; }
        public LoginModel LoginModel { get; set; }
    }

    public class RegisterModel : IMapFrom<User>
    {
        [Required(ErrorMessage = "Please, provide full name.")]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please, provide phone number.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please, provide email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please, provide valid email address.")]
        [RegularExpression("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Please, provide valid email address.")]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, provide company name.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Please, provide password.")]
        [DataType(DataType.Password, ErrorMessage = "Please, provide valid password.")]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please, provide valid password.")]
        [Compare("Password", ErrorMessage = "Password cannot matched! Try again.")]
        public string ConfirmPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, RegisterModel>();
            profile.CreateMap<RegisterModel, User>();
        }
    }

    public class LoginModel : IMapFrom<User>
    {
        [Required(ErrorMessage = "Please, provide email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please, provide valid email address.")]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, provide password.")]
        [DataType(DataType.Password, ErrorMessage = "Please, provide valid password.")]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, LoginModel>();
            profile.CreateMap<LoginModel, User>();
        }
    }

    public class UserModel : IMapFrom<User>
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int? EmployeeId { get; set; }
        public string? Image { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserModel>();
        }
    }
}