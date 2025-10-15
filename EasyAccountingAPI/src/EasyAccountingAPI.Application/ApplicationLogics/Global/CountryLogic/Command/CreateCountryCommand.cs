namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class CreateCountryCommand : CountryCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateCountryCommand, bool>
        {
            private readonly ICountryManager _countryManager;
            private readonly IMapper _mapper;

            public Handler(ICountryManager countryManager, IMapper mapper)
            {
                _countryManager = countryManager;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
            {
                var createdCountry = _mapper.Map<Country>(request);
                createdCountry = await _countryManager.CreateAsync(createdCountry);

                if(createdCountry is not null && createdCountry.Id > 0)
                    return true;

                return false;
            }
        }
    }
}