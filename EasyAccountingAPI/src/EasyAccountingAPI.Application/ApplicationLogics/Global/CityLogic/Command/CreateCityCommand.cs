namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CityLogic.Command
{
    public class CreateCityCommand : CityCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateCityCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICityRepository _cityRepository;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, ICityRepository cityRepository, IMapper mapper)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _cityRepository = cityRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateCityCommand request, CancellationToken cancellationToken)
            {
                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Create city
                    var city = _mapper.Map<City>(request);
                    await _cityRepository.CreateAsync(city, cancellationToken);

                    // Final save + commit
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return city.Id > 0;
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