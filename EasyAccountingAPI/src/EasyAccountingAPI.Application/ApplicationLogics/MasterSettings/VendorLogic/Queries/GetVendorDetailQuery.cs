namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VendorLogic.Queries
{
    public class GetVendorDetailQuery : IRequest<VendorUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetVendorDetailQuery, VendorUpdateModel>
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

            public async Task<VendorUpdateModel> Handle(GetVendorDetailQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check if the vendor id is null, empty, whitespace, or equals to -1
                if (string.IsNullOrEmpty(request.Id) || string.IsNullOrWhiteSpace(request.Id) || request.Id == "-1")
                    return new VendorUpdateModel();

                // Decrypt the vendor id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var vendorId))
                    return new VendorUpdateModel();

                // Get vendor by id
                var getVendor = await _vendorRepository.GetByIdAsync(vendorId, cancellationToken);

                if (getVendor is null)
                    return new VendorUpdateModel();

                // Map vendor
                var mapVendor = _mapper.Map<VendorUpdateModel>(getVendor);
                return mapVendor;
            }
        }
    }
}