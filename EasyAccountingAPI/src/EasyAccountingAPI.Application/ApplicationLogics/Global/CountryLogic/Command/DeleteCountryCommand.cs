namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class DeleteCountryCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteCountryCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICountryRepository _countryRepository;
            private readonly ICityRepository _cityRepository;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, ICountryRepository countryRepository, ICityRepository cityRepository)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _countryRepository = countryRepository;
                _cityRepository = cityRepository;
            }

            public async Task<bool> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
            {
                // Decrypt the country ID
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var countryId))
                    return false;

                // Fetch the country
                var country = await _countryRepository.GetByIdAsync(countryId, cancellationToken);
                if (country is null)
                    return false;

                // Begin transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // 1. Bulk soft delete cities (NO SaveChanges inside)
                    await _cityRepository.DeleteBulkCityByCountryIdAsync(country.Id, cancellationToken);

                    // 2. Soft delete country (NO SaveChanges inside)
                    country.IsDeleted = true;
                    country.DeletedDateTime = DateTime.UtcNow;
                    _countryRepository.Update(country);

                    // 3. Single commit
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