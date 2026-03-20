namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.PurchaseLogic.Command
{
    public class CreatePurchaseCommand : PurchaseCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreatePurchaseCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IPurchaseRepository _purchaseRepository;
            private readonly IMapper _mapper;

            public Handler(IHttpContextAccessor httpContextAccessor, 
                IUnitOfWorkRepository unitOfWorkRepository, 
                IPurchaseRepository purchaseRepository, 
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _purchaseRepository = purchaseRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
            {
                // Get the user ID from the HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Create Purchase
                    var purchase = _mapper.Map<Purchas>(request);
                    await _purchaseRepository.CreateAsync(purchase, cancellationToken);

                    // Final save + commit
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return purchase.Id > 0;
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