namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CityLogic.Command
{
    public class UpdateCityCommand : CityUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateCityCommand, bool>
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

            public async Task<bool> Handle(UpdateCityCommand request, CancellationToken ct)
            {
                // Fetch existing city
                var getExistingCity = await _cityRepository.GetByIdAsync(request.Id, ct);
                if (getExistingCity is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(ct);

                try
                {
                    _mapper.Map((CityUpdateModel)request, getExistingCity);
                    _cityRepository.Update(getExistingCity);

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