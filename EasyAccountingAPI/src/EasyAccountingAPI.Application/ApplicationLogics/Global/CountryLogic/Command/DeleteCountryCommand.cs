namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class DeleteCountryCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteCountryCommand, bool>
        {
            private readonly IUnitOfWorkManager _unitOfWorkManager;

            public Handler(IUnitOfWorkManager unitOfWorkManager)
            {
                _unitOfWorkManager = unitOfWorkManager;
            }

            public async Task<bool> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
            {
                // Decrypt the country ID
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var countryId))
                    return false;

                // Get repositories
                var countryManager = _unitOfWorkManager.GetRepository<ICountryManager>();
                var cityManager = _unitOfWorkManager.GetRepository<ICityManager>();

                // Fetch the country
                var country = await countryManager.GetByIdAsync(countryId);
                if (country is null)
                    return false;

                // Begin transaction
                await _unitOfWorkManager.BeginTransactionAsync(cancellationToken);

                try
                {
                    // 1. Bulk soft delete cities (NO SaveChanges inside)
                    await cityManager.DeleteBulkCityByCountryIdAsync(country.Id);

                    // 2. Soft delete country (NO SaveChanges inside)
                    country.IsDeleted = true;
                    country.DeletedDateTime = DateTime.UtcNow;
                    await countryManager.UpdateAsync(country);

                    // 3. Single commit
                    await _unitOfWorkManager.CommitAsync(cancellationToken);
                    return true;
                }
                catch
                {
                    await _unitOfWorkManager.RollbackAsync(cancellationToken);
                    throw; 
                }
            }
        }
    }
}