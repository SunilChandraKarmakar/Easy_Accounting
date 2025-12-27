namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class CreateCountryCommand : CountryCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateCountryCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICountryRepository _countryRepository;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, ICountryRepository countryRepository, IMapper mapper)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _countryRepository = countryRepository;
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