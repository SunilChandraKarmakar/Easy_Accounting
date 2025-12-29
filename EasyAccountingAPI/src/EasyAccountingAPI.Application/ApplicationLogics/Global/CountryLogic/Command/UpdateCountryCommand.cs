namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class UpdateCountryCommand : CountryUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateCountryCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICountryRepository _countryRepository;
            private readonly ICityRepository _cityRepository;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, ICountryRepository countryRepository, ICityRepository cityRepository, IMapper mapper)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _countryRepository = countryRepository;
                _cityRepository = cityRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateCountryCommand request, CancellationToken ct)
            {
                // Fetch existing country
                var getExistingCountry = await _countryRepository.GetByIdAsync(request.Id, ct);
                if (getExistingCountry is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(ct);

                try
                {
                    _mapper.Map((CountryUpdateModel)request, getExistingCountry);
                    _countryRepository.Update(getExistingCountry);

                    if (request.Cities is not null && request.Cities.Count > 0)
                    {
                        getExistingCountry.Cities ??= new List<City>();

                        foreach (var city in request.Cities)
                        {
                            if (city.Id <= 0)
                            {
                                if (string.IsNullOrWhiteSpace(city.Name)) continue;

                                var newCity = new City
                                {
                                    Name = city.Name.Trim(),
                                    CountryId = getExistingCountry.Id
                                };

                                // Add via navigation OR repository. Either is fine; nav is simplest.
                                getExistingCountry.Cities.Add(newCity);
                                continue;
                            }

                            var existingCity = getExistingCountry.Cities.FirstOrDefault(x => x.Id == city.Id);

                            // If not loaded, fetch tracked from CityRepo
                            if (existingCity is null)
                            {
                                existingCity = await _cityRepository.GetByIdAsync(city.Id, ct);
                                if (existingCity is null) continue;

                                // Safety: ensure it belongs to this country
                                if (existingCity.CountryId != getExistingCountry.Id)
                                    continue; // or throw
                            }

                            if (!string.IsNullOrWhiteSpace(city.Name))
                                existingCity.Name = city.Name.Trim();

                            existingCity.CountryId = getExistingCountry.Id;
                        }
                    }

                    await _unitOfWorkRepository.SaveChangesAsync(ct);
                    await _unitOfWorkRepository.CommitTransactionAsync(ct);
                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(ct);
                    return false;
                }
            }
        }
    }
}