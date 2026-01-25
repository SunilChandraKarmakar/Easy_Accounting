namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ModuleLogic.Queries
{
    public class GetModulesByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<ModuleGridModel>>
    {
        public class Handler : IRequestHandler<GetModulesByFilterQuery, FilterPageResultModel<ModuleGridModel>>
        {
            private readonly IModuleRepository _moduleRepository;
            private readonly IMapper _mapper;

            public Handler(IModuleRepository moduleRepository, IMapper mapper)
            {
                _moduleRepository = moduleRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<ModuleGridModel>> Handle(GetModulesByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Get module and map to grid model
                var getModules = await _moduleRepository.GetModulesByFilterAsync(request, cancellationToken);
                var mapModules = _mapper.Map<ICollection<ModuleGridModel>>(getModules.Items);

                // Return paginated result
                return new FilterPageResultModel<ModuleGridModel>(mapModules, getModules.TotalCount);
            }
        }
    }
}