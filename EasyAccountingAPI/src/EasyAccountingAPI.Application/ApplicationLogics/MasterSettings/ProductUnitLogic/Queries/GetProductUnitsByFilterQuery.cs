namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ProductUnitLogic.Queries
{
    public class GetProductUnitsByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<ProductUnitGridModel>>
    {
        public class Handler : IRequestHandler<GetProductUnitsByFilterQuery, FilterPageResultModel<ProductUnitGridModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IProductUnitRepository _productUnitRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IProductUnitRepository productUnitRepository, 
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _productUnitRepository = productUnitRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<ProductUnitGridModel>> Handle(GetProductUnitsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get product unit and map to grid model
                var getProductUnits = await _productUnitRepository.GetProductUnitsByFilterAsync(request, cancellationToken);
                var mapProductUnits = _mapper.Map<ICollection<ProductUnitGridModel>>(getProductUnits.Items);

                // Return paginated result
                return new FilterPageResultModel<ProductUnitGridModel>(mapProductUnits, getProductUnits.TotalCount);
            }
        }
    }
}