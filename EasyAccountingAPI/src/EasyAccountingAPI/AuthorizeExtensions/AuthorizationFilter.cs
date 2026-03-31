using EasyAccountingAPI.Repository.Contracts.MasterSettings.AccessControl;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EasyAccountingAPI.AuthorizeExtensions
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFeatureRepository _featureRepository;
        private readonly IActionRepository _actionRepository;
        private readonly IEmployeeFeatureActionRepository _employeeFeatureActionRepository;
        private readonly UserManager<User> _userManager;

        private readonly string _controllerName;
        private readonly string _actionName;

        public AuthorizationFilter(
            IHttpContextAccessor httpContextAccessor,
            IFeatureRepository featureRepository,
            IActionRepository actionRepository,
            IEmployeeFeatureActionRepository employeeFeatureActionRepository,
            UserManager<User> userManager,
            string controllerName, 
            string actionName)
        {
            _httpContextAccessor = httpContextAccessor;
            _featureRepository = featureRepository;
            _actionRepository = actionRepository;
            _employeeFeatureActionRepository = employeeFeatureActionRepository;
            _userManager = userManager;
            _controllerName = controllerName;
            _actionName = actionName;            
        }
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Get login user id
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? null;

            // Get feature 
            var feature = await _featureRepository.GetFeatureByControllerName(_controllerName);

            // Get action
            var action = await _actionRepository.GetActionByName(_actionName);

            // Check login user have permission
            if (userId is not null && feature is not null && action is not null)
            {
                // Get login user info by id
                var loginUserInfo = await _userManager.FindByIdAsync(userId);

                var havePermission = await _employeeFeatureActionRepository.GetEmployeeFeatureActionByEmployeeAndFeatureAndActionAsync(userId, feature.Id, action.Id);

                if (havePermission is null || (loginUserInfo is not null && loginUserInfo.ForcePasswordChanged == false))
                    context.Result = new ForbidResult();
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }
    }