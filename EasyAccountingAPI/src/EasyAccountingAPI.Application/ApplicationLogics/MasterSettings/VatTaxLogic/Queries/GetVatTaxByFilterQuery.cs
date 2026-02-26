namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VatTaxLogic.Queries
{
    public class GetVatTaxByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<VatTaxGridModel>>
    {
        public class Handler : IRequestHandler<GetVatTaxByFilterQuery, FilterPageResultModel<VatTaxGridModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IVatTaxRepository _vatTaxRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor, 
                IVatTaxRepository vatTaxRepository, 
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _vatTaxRepository = vatTaxRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<VatTaxGridModel>> Handle(GetVatTaxByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get vat tax and map to grid model
                var getVatTax = await _vatTaxRepository.GetVatTaxesByFilterAsync(request, userId, cancellationToken);

                // Map vat tax
                var mapVatTax = _mapper.Map<ICollection<VatTaxGridModel>>(getVatTax.Items);

                // Return paginated result
                return new FilterPageResultModel<VatTaxGridModel>(mapVatTax, getVatTax.TotalCount);
            }
        }
    }
}