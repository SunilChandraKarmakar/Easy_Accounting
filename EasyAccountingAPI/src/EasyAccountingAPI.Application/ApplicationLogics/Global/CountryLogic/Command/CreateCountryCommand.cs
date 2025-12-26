namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class CreateCountryCommand : CountryCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateCountryCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICountryRepository _countryRepository;
            private readonly ICityRepository _cityRepository;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, ICountryRepository countryRepository, ICityRepository cityRepository,
                IMapper mapper)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _countryRepository = countryRepository;
                _cityRepository = cityRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
            {
                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Create Country
                    var country = _mapper.Map<Country>(request);
                    await _countryRepository.CreateAsync(country, cancellationToken);

                    // Create Cities (if any)
                    if (request.Cities?.Any() == true)
                    {
                        var cities = request.Cities
                            .Select(cityCreateModel =>
                            {
                                var city = _mapper.Map<City>(cityCreateModel);
                                city.Country = country;
                                return city;
                            })
                            .ToList();

                        await _cityRepository.BulkCreateAsync(cities, cancellationToken);
                    }

                    // Final save + commit
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return country.Id > 0;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    return false;
                }
            }
        }
    }
}