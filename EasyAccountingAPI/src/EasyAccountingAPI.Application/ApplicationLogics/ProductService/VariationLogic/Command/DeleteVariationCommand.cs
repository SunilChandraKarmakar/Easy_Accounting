namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.VariationLogic.Command
{
    public class DeleteVariationCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteVariationCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IVariationRepository _variationRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IVariationRepository variationRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _variationRepository = variationRepository;
            }

            public async Task<bool> Handle(DeleteVariationCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the variation id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var variationId))
                    return false;

                // Fetch the variation
                var variation = await _variationRepository.GetByIdAsync(variationId, cancellationToken);
                if (variation is null)
                    return false;

                variation.IsDeleted = true;
                variation.DeletedDateTime = DateTime.UtcNow;

                _variationRepository.Update(variation);
                return await _unitOfWorkRepository.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}