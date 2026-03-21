namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.PurchaseLogic.Command
{
    public class DeletePurchaseCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeletePurchaseCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IPurchaseRepository _purchaseRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IPurchaseRepository purchaseRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _purchaseRepository = purchaseRepository;
            }

            public async Task<bool> Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the purchase id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var purchaseId))
                    return false;

                // Fetch the purchase
                var purchaseOrder = await _purchaseRepository.GetByIdAsync(purchaseId, cancellationToken);
                if (purchaseOrder is null)
                    return false;

                purchaseOrder.IsDeleted = true;
                purchaseOrder.DeletedDateTime = DateTime.UtcNow;

                // Update the purchase items
                foreach (var purchaseItem in purchaseOrder.PurchaseItems)
                {
                    purchaseItem.IsDeleted = true;
                    purchaseItem.DeletedDateTime = DateTime.UtcNow;
                }

                _purchaseRepository.Update(purchaseOrder);
                return await _unitOfWorkRepository.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}