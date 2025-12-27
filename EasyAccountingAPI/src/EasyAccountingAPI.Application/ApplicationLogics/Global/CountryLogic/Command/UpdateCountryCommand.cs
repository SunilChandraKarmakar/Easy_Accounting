namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class UpdateCountryCommand : CountryUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateCountryCommand, bool>
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

            public async Task<bool> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
            {
                // Get exist country
                var getCountry = await _countryRepository.GetByIdAsync(request.Id);

                if (getCountry is null)
                    return false;

                // Begin transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Map and update country
                    getCountry = _mapper.Map((CountryUpdateModel)request, getCountry);
                    _countryRepository.Update(getCountry);

                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch (Exception ex)
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    return false;
                }
            }
        }
    }
}