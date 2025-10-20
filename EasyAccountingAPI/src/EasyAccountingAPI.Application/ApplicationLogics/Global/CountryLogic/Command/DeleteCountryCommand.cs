namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class DeleteCountryCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<DeleteCountryCommand, bool>
        {
            private readonly ICountryManager _countryManager;
            public Handler(ICountryManager countryManager) => _countryManager = countryManager;

            public async Task<bool> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
            {
                //// Check, id is valid
                //if (request.Id <= 0)
                //    return false;

                //// Get exist country
                //var getCountry = await _countryManager.GetByIdAsync(request.Id);

                //if (getCountry is null)
                //    return false;

                //// Check, country is not null
                //if (getCountry is not null)
                //{
                //    getCountry.IsDeleted = true;
                //    getCountry.DeletedDateTime = DateTime.UtcNow;
                //    await _countryManager.UpdateAsync(getCountry);

                //    return true;
                //}

                return false;
            }
        }
    }
}