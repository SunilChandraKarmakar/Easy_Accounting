namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Queries
{
    public class GetCountryDetailQuery : IRequest<CountryUpdateModel>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetCountryDetailQuery, CountryUpdateModel>
        {
            private readonly ICountryManager _countryManager;
            private readonly IMapper _mapper;

            public Handler(ICountryManager countryManager, IMapper mapper)
            {
                _countryManager = countryManager;
                _mapper = mapper;
            }

            public async Task<CountryUpdateModel> Handle(GetCountryDetailQuery request, CancellationToken cancellationToken)
            {
                // Check, id is valid
                if (request.Id <= 0)
                    return new CountryUpdateModel();

                // Get country by id
                var getCountry = await _countryManager.GetByIdAsync(request.Id);

                if (getCountry is null)
                    return new CountryUpdateModel();

                // Map country
                var mapCountry = _mapper.Map<CountryUpdateModel>(getCountry);
                return mapCountry;
            }
        }
    }
}