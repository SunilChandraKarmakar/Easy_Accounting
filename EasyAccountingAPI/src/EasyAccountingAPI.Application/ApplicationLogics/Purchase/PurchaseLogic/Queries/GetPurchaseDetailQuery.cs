namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.PurchaseLogic.Queries
{
    public class GetPurchaseDetailQuery : IRequest<PurchaseUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetPurchaseDetailQuery, PurchaseUpdateModel>
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

            public async Task<PurchaseUpdateModel> Handle(GetPurchaseDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check if the purchase id is null, empty, whitespace, or equals to -1
                if (string.IsNullOrEmpty(request.Id) || string.IsNullOrWhiteSpace(request.Id) || request.Id == "-1")
                    return new PurchaseUpdateModel();

                // Decrypt the purchase id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var purchaseId))
                    return new PurchaseUpdateModel();

                // Get purchase by id
                var getPurchase = await _purchaseRepository.GetByIdAsync(purchaseId, cancellationToken);

                if (getPurchase is null)
                    return new PurchaseUpdateModel();

                // Map purchase order
                var mapPurchaseOrder = _mapper.Map<PurchaseUpdateModel>(getPurchase);
                return mapPurchaseOrder;
            }
        }
    }
}