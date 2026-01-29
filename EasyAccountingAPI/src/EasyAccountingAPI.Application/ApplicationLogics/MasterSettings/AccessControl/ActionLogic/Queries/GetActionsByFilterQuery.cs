using EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.ActionLogic.Model;

namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.ActionLogic.Queries
{
    public class GetActionsByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<ActionGridModel>>
    {
        public class Handler : IRequestHandler<GetActionsByFilterQuery, FilterPageResultModel<ActionGridModel>>
        {
            private readonly IActionRepository _actionRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;

            public Handler(IActionRepository actionRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
            {
                _actionRepository = actionRepository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;                
            }

            public async Task<FilterPageResultModel<ActionGridModel>> Handle(GetActionsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get actions and map to grid model
                var getActions = await _actionRepository.GetActionsByFilterAsync(request, cancellationToken);
                var mapActions = _mapper.Map<ICollection<ActionGridModel>>(getActions.Items);

                // Return paginated result
                return new FilterPageResultModel<ActionGridModel>(mapActions, getActions.TotalCount);
            }
        }
    }
}