namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.PurchaseLogic.Command
{
    public class UpdatePurchaseCommand : PurchaseUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdatePurchaseCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IPurchaseRepository _purchaseRepository;
            private readonly IPurchaseItemRepository _purchaseItemRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IPurchaseRepository purchaseRepository,
                IPurchaseItemRepository purchaseItemRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _purchaseRepository = purchaseRepository;
                _purchaseItemRepository = purchaseItemRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing purchase
                var getPurchase = await _purchaseRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getPurchase is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // If the request indicates no purchase item but the purchase currently has item,
                    // delete those dependent entities to avoid leaving orphaned/invalid relationships.
                    if (getPurchase.PurchaseItems != null && getPurchase.PurchaseItems.Any())
                    {
                        // Copy to list to avoid modifying collection during enumeration
                        var existingPurchaseItem = getPurchase.PurchaseItems.ToList();
                        foreach (var purchaseItem in existingPurchaseItem)
                            _purchaseItemRepository.Delete(purchaseItem);
                    }

                    _mapper.Map((PurchaseUpdateModel)request, getPurchase);
                    getPurchase.UpdatedById = userId;
                    getPurchase.UpdatedDateTime = DateTime.UtcNow;

                    _purchaseRepository.Update(getPurchase);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    return false;
                }
            }
        }
    }
}