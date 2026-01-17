namespace EasyAccountingAPI.Controllers.MasterSettings
{
    public class InvoiceSettingController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<InvoiceSettingGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<InvoiceSettingGridModel>>> GetFilterInvoiceSettingsAsync(
            GetInvoiceSettingsByFilterQuery getinvoiceSettingsByFilterQuery)
        {
            var getFilterInvoiceSettings = await Mediator.Send(getinvoiceSettingsByFilterQuery);
            return Ok(getFilterInvoiceSettings);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InvoiceSettingViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<InvoiceSettingViewModel>> GetByIdAsync(string id)
        {
            var invoiceSettingVm = new InvoiceSettingViewModel
            {
                UpdateModel = await Mediator.Send(new GetInvoiceSettingDetailQuery { Id = id }),
            };

            return Ok(invoiceSettingVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateInvoiceSettingCommand createInvoiveSettingCommand)
        {
            if (ModelState.IsValid)
            {
                var isInvoiceSettingCreate = await Mediator.Send(createInvoiveSettingCommand);
                return Ok(isInvoiceSettingCreate);
            }

            return BadRequest(createInvoiveSettingCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateInvoiceSettingCommand updateInvoiceSettingCommand)
        {
            if (ModelState.IsValid)
            {
                var isInvoiceSettingUpdate = await Mediator.Send(updateInvoiceSettingCommand);
                return Ok(isInvoiceSettingUpdate);
            }

            return BadRequest(updateInvoiceSettingCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteInvoiceSetting = await Mediator.Send(new DeleteInvoiceSettingCommand { Id = id });
            return Ok(isDeleteInvoiceSetting);
        }
    }
}