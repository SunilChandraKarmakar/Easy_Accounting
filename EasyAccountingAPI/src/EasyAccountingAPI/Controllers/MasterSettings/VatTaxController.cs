namespace EasyAccountingAPI.Controllers.MasterSettings
{
    public class VatTaxController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<VatTaxGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<VatTaxGridModel>>> GetFilterVatTaxesAsync(
            GetVatTaxByFilterQuery getVatTaxByFilterQuery)
        {
            var getFilterVatTax = await Mediator.Send(getVatTaxByFilterQuery);
            return Ok(getFilterVatTax);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VatTaxViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<VatTaxViewModel>> GetByIdAsync(string id)
        {
            var vatTaxVm = new VatTaxViewModel
            {
                UpdateModel = await Mediator.Send(new GetVatTaxDetailQuery { Id = id }),
            };

            // Set select list
            vatTaxVm.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());

            return Ok(vatTaxVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateVatTaxCommand createVatTaxCommand)
        {
            if (ModelState.IsValid)
            {
                var isVatTaxCreate = await Mediator.Send(createVatTaxCommand);
                return Ok(isVatTaxCreate);
            }

            return BadRequest(createVatTaxCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateVatTaxCommand updateVatTaxCommand)
        {
            if (ModelState.IsValid)
            {
                var isVatTaxUpdate = await Mediator.Send(updateVatTaxCommand);
                return Ok(isVatTaxUpdate);
            }

            return BadRequest(updateVatTaxCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteVatTax = await Mediator.Send(new DeleteVatTaxCommand { Id = id });
            return Ok(isDeleteVatTax);
        }
    }
}