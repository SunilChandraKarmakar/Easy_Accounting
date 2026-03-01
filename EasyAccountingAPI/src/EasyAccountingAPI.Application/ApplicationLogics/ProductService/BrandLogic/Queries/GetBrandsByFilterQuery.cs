namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.BrandLogic.Queries
{
    public class GetBrandsByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<BrandGridModel>>
    {
        public class Handler : IRequestHandler<GetBrandsByFilterQuery, FilterPageResultModel<BrandGridModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IBrandRepository brandRepository, 
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _brandRepository = brandRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<BrandGridModel>> Handle(GetBrandsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get brand and map to grid model
                var getBrands = await _brandRepository.GetBrandsByFilterAsync(request, userId, cancellationToken);
                var mapBrands = _mapper.Map<ICollection<BrandGridModel>>(getBrands.Items);

                // Return paginated result
                return new FilterPageResultModel<BrandGridModel>(mapBrands, getBrands.TotalCount);
            }
        }
    }
}