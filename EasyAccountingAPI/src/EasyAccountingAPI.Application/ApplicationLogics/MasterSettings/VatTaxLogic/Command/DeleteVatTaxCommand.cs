namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VatTaxLogic.Command
{
    public class DeleteVatTaxCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteVatTaxCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IVatTaxRepository _vatTaxRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor, 
                IUnitOfWorkRepository unitOfWorkRepository,
                IVatTaxRepository vatTaxRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _vatTaxRepository = vatTaxRepository;
            }

            public async Task<bool> Handle(DeleteVatTaxCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the vat tax id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var vatTaxId))
                    return false;

                // Fetch the vat tax
                var vatTax = await _vatTaxRepository.GetByIdAsync(vatTaxId, cancellationToken);
                if (vatTax is null)
                    return false;

                // Begin transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    vatTax.IsDeleted = true;
                    vatTax.DeletedDateTime = DateTime.UtcNow;
                    _vatTaxRepository.Update(vatTax);

                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}