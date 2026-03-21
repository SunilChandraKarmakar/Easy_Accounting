namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.PurchaseLogic.Queries
{
    public class SelectListPurchaseQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListPurchaseQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IPurchaseRepository _purchaseRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IPurchaseRepository purchaseRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _purchaseRepository = purchaseRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListPurchaseQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getPurchases = await _purchaseRepository.GetPurchaseSelectList(userId, cancellationToken);
                return getPurchases;
            }
        }
    }
}