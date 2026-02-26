namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VatTaxLogic.Queries
{
    public class GetVatTaxDetailQuery : IRequest<VatTaxUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetVatTaxDetailQuery, VatTaxUpdateModel>
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

            public async Task<VatTaxUpdateModel> Handle(GetVatTaxDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                if(request.Id == "-1")
                    return new VatTaxUpdateModel();

                // Decrypt the vat tax id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var vatTaxId))
                    return new VatTaxUpdateModel();

                // Get vat tax by id
                var getVatTax = await _vatTaxRepository.GetByIdAsync(vatTaxId, cancellationToken);

                if (getVatTax is null)
                    return new VatTaxUpdateModel();

                // Map vat tax
                var mapVatTax = _mapper.Map<VatTaxUpdateModel>(getVatTax);
                return mapVatTax;
            }
        }
    }
}