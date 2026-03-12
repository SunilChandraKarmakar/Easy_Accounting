namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VendorLogic.Queries
{
    public class GetVendorsByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<VendorGridModel>>
    {
        public class Handler : IRequestHandler<GetVendorsByFilterQuery, FilterPageResultModel<VendorGridModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IVendorRepository _vendorRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IVendorRepository vendorRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _vendorRepository = vendorRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<VendorGridModel>> Handle(GetVendorsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get vendor and map to grid model
                var getVendors = await _vendorRepository.GetVendorsByFilterAsync(request, userId, cancellationToken);
                var mapVendors = _mapper.Map<ICollection<VendorGridModel>>(getVendors.Items);

                // Return paginated result
                return new FilterPageResultModel<VendorGridModel>(mapVendors, getVendors.TotalCount);
            }
        }
    }
}