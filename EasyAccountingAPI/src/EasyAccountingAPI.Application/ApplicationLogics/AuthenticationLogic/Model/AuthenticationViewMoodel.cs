namespace EasyAccountingAPI.Application.ApplicationLogics.AuthenticationLogic.Model
{
    public class AuthenticationViewMoodel
    {
        public RegisterModel RegisterModel { get; set; }
    }

    public class RegisterModel : IMapFrom<User>
    {
        [Required(ErrorMessage = "Please, provide email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please, provide valid email address.")]
        [RegularExpression("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Please, provide valid email address.")]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, provide password.")]
        [DataType(DataType.Password, ErrorMessage = "Please, provide valid password.")]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please, provide valid password.")]
        [Compare("Password", ErrorMessage = "Password cannot matched! Try again.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please, provide full name.")]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string FullName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, RegisterModel>();
            profile.CreateMap<RegisterModel, User>();
        }
    }
}