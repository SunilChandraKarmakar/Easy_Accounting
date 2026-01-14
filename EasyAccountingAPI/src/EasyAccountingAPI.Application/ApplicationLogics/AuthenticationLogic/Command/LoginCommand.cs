namespace EasyAccountingAPI.Application.ApplicationLogics.AuthenticationLogic.Command
{
    public class LoginCommand : LoginModel, IRequest<UserModel>
    {
        public class Handler : IRequestHandler<LoginCommand, UserModel>
        {
            private readonly UserManager<User> _userManager;
            private readonly IOptions<AppSettings> _appSeeting;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUserLoginHistoryRepository _userLoginHistoryRepository;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IEmployeeRoleRepository _employeeRoleRepository;

            public Handler(UserManager<User> userManager, IOptions<AppSettings> appSeeting, IMapper mapper, 
                IHttpContextAccessor httpContextAccessor, IUserLoginHistoryRepository userLoginHistoryRepository,
                IUnitOfWorkRepository unitOfWorkRepository, IEmployeeRoleRepository employeeRoleRepository)
            {
                _userManager = userManager;
                _appSeeting = appSeeting;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
                _userLoginHistoryRepository = userLoginHistoryRepository;
                _unitOfWorkRepository = unitOfWorkRepository;
                _employeeRoleRepository = employeeRoleRepository;
            }

            public async Task<UserModel> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                // Check email exist or not
                var existUser = await _userManager.FindByEmailAsync(request.Email);

                // Check exist user
                if (existUser is null)
                    throw new Exception("We couldn’t find an account with the provided email address.");

                // Check disable login access for this employee
                if (existUser is not null && existUser.LockoutEnd is not null)
                    throw new Exception("Login access is currently disabled. Please reach out to the administrator for support.");

                if (existUser is not null && await _userManager.CheckPasswordAsync(existUser, request.Password))
                {
                    var mapExistUser = _mapper.Map<UserModel>(existUser);

                    var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSeeting.Value.JWTSecret));
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, existUser.Id.ToString()),
                            new Claim("UserName", existUser.UserName!.ToString()),
                            new Claim("FullName", existUser.FullName!.ToString()),
                            new Claim("EmployeeId",(existUser.EmployeeId == null) ? 0.ToString() : existUser.EmployeeId.ToString()!)
                        }),

                        Expires = DateTime.UtcNow.AddHours(10),
                        SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);

                    // Set login user token
                    mapExistUser.Token = token;

                    //Get User IP Address
                    var clientLoginIp = IPHelper.GetIpAddress(_httpContextAccessor.HttpContext!);

                    //Save User IP Address in DB
                    var createUserLoginHistoryModel = new UserLoginHistory
                    {
                        UserId = existUser.Id,
                        LoginIp = clientLoginIp,
                        LoginDateTime = DateTime.UtcNow
                    };

                    await _userLoginHistoryRepository.CreateAsync(createUserLoginHistoryModel, cancellationToken);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                    // Get login user employee role name by employee id
                    mapExistUser.RoleName = await _employeeRoleRepository.GetEmployeeRoleNameByEmployeeId((int)mapExistUser.EmployeeId!);

                    return mapExistUser;
                }

                throw new Exception("Email and password cannot matched! Please, try again.");
            }
        }
    }
}