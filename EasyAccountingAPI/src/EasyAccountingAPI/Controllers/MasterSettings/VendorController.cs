namespace EasyAccountingAPI.Controllers.MasterSettings
{
    public class VendorController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<VendorGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<VendorGridModel>>> GetFilterVendorsAsync(
            GetVendorsByFilterQuery getVendorsByFilterQuery)
        {
            var getFilterVendors = await Mediator.Send(getVendorsByFilterQuery);
            return Ok(getFilterVendors);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VendorViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<VendorViewModel>> GetByIdAsync(string id)
        {
            var vendorVm = new VendorViewModel
            {
                UpdateModel = await Mediator.Send(new GetVendorDetailQuery { Id = id }),
            };

            // Select list
            vendorVm.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());
            vendorVm.OptionsDataSources.CountrySelectList = await Mediator.Send(new SelectListCountryQuery());

            return Ok(vendorVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateVendorCommand createVendorCommand)
        {
            if (ModelState.IsValid)
            {
                var isVendorCreated = await Mediator.Send(createVendorCommand);
                return Ok(isVendorCreated);
            }

            return BadRequest(createVendorCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateVendorCommand updateVendorCommand)
        {
            if (ModelState.IsValid)
            {
                var isVendorUpdate = await Mediator.Send(updateVendorCommand);
                return Ok(isVendorUpdate);
            }

            return BadRequest(updateVendorCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteVendor = await Mediator.Send(new DeleteVendorCommand { Id = id });
            return Ok(isDeleteVendor);
        }

        [HttpGet("{companyId}")]
        [ProducesResponseType(typeof(IEnumerable<SelectModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SelectModel>>> GetVendorByCompanyIdAsync(int companyId)
        {
            var vendors = await Mediator.Send(new SelectListVendorByCompanyIdQuery { CompanyId = companyId });
            return Ok(vendors);
        }
    }
}