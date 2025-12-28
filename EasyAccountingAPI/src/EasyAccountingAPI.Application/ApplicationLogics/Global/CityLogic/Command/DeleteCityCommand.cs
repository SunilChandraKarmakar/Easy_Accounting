namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CityLogic.Command
{
    public class DeleteCityCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<DeleteCityCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICityRepository _cityRepository;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, ICityRepository cityRepository)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _cityRepository = cityRepository;
            }

            public async Task<bool> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
            {

                // Fetch the city
                var city = await _cityRepository.GetByIdAsync(request.Id);
                if (city is null)
                    return false;

                // Begin transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    city.IsDeleted = true;
                    city.DeletedDateTime = DateTime.UtcNow;
                    _cityRepository.Update(city);

                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
                    return true;
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