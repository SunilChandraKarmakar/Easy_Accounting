namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CityLogic.Queries
{
    public class GetCityDetailQuery : IRequest<CityUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetCityDetailQuery, CityUpdateModel>
        {
            private readonly ICityRepository _cityRepository;
            private readonly IMapper _mapper;

            public Handler(ICityRepository cityRepository, IMapper mapper)
            {
                _cityRepository = cityRepository;
                _mapper = mapper;
            }

            public async Task<CityUpdateModel> Handle(GetCityDetailQuery request, CancellationToken cancellationToken)
            {
                // Decrypt the city id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var cityId))
                    return new CityUpdateModel();

                // Get city by id
                var getCity = await _cityRepository.GetByIdAsync(cityId, cancellationToken);

                if (getCity is null)
                    return new CityUpdateModel();

                // Map city
                var mapCity = _mapper.Map<CityUpdateModel>(getCity);
                return mapCity;
            }
        }
    }
}