using EasyAccountingAPI.Shared.ErrorMessages;
using EasyAccountingAPI.Shared.Exceptions;
using EasyAccountingAPI.Shared.Services;

namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class DeleteCountryCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteCountryCommand, bool>
        {
            private readonly ICountryManager _countryManager;
            public Handler(ICountryManager countryManager) => _countryManager = countryManager;

            public async Task<bool> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
            {
                // Decrypt country id
                var decryptCountryId = EncryptionService.Decrypt(request.Id);

                // Check if country decrypt id is null
                if (string.IsNullOrWhiteSpace(decryptCountryId) || string.IsNullOrEmpty(decryptCountryId))
                    return false;

                // Convert decrypt country id
                var convertCountryId = Convert.ToInt32(decryptCountryId);

                // Get 
                var getCompliance = await _complianceManager.GetByIdAsync(convertId);
                if (getCompliance is null)
                    throw new BadRequestException(ProvideErrorMessage.ComplianceNotFound);

                // Get exist country
                var getCountry = await _countryManager.GetByIdAsync(request.Id);

                if (getCountry is null)
                    return false;

                // Check, country is not null
                if (getCountry is not null)
                {
                    getCountry.IsDeleted = true;
                    getCountry.DeletedDateTime = DateTime.UtcNow;
                    await _countryManager.UpdateAsync(getCountry);

                    return true;
                }

                return false;
            }
        }
    }
}