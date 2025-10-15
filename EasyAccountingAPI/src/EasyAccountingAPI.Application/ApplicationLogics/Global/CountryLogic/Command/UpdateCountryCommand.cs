namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class UpdateCountryCommand : CountryUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateCountryCommand, bool>
        {
            private readonly ICountryManager _countryManager;
            private readonly IMapper _mapper;

            public Handler(ICountryManager countryManager, IMapper mapper)
            {
                _countryManager = countryManager;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
            {
                // Check, id is valid
                if (request.Id <= 0)
                    return false;

                // Get exist country
                var getCountry = await _countryManager.GetByIdAsync(request.Id);

                if (getCountry is null)
                    return false;

                // Map and update country
                getCountry = _mapper.Map((CountryUpdateModel)request, getCountry);
                getCountry = await _countryManager.UpdateAsync(getCountry);

                if(getCountry is not null && getCountry.Id > 0)
                    return true;

                return false;
            }
        }
    }
}