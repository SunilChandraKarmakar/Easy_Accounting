namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.PurchaseLogic.Queries
{
    public class GetPurchasesByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<PurchaseGridModel>>
    {
        public class Handler : IRequestHandler<GetPurchasesByFilterQuery, FilterPageResultModel<PurchaseGridModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IPurchaseRepository _purchaseRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IPurchaseRepository purchaseRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _purchaseRepository = purchaseRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<PurchaseGridModel>> Handle(GetPurchasesByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get purchase and map to grid model
                var getPurchases = await _purchaseRepository.GetPurchasesByFilterAsync(request, userId, cancellationToken);
                var mapPurchases = _mapper.Map<ICollection<PurchaseGridModel>>(getPurchases.Items);

                // Return paginated result
                return new FilterPageResultModel<PurchaseGridModel>(mapPurchases, getPurchases.TotalCount);
            }
        }
    }
}