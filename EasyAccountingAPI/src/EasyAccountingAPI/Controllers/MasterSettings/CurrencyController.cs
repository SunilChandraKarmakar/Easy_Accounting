namespace EasyAccountingAPI.Controllers.MasterSettings
{
    public class CurrencyController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<CurrencyGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<CurrencyGridModel>>> GetFilterCurrenciesAsync(
            GetCurrenciesByFilterQuery getCurrenciesByFilterQuery)
        {
            var getFilterCurrencies = await Mediator.Send(getCurrenciesByFilterQuery);
            return Ok(getFilterCurrencies);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CurrencyViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CurrencyViewModel>> GetByIdAsync(string id)
        {
            var currencyVm = new CurrencyViewModel
            {
                UpdateModel = await Mediator.Send(new GetCurrencyDetailQuery { Id = id })
            };

            return Ok(currencyVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateCurrencyCommand createCurrencyCommand)
        {
            if (ModelState.IsValid)
            {
                var isCurrencyCreated = await Mediator.Send(createCurrencyCommand);
                return Ok(isCurrencyCreated);
            }

            return BadRequest(createCurrencyCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateCurrencyCommand updateCurrencyCommand)
        {
            if (ModelState.IsValid)
            {
                var isCUrrencyUpdate = await Mediator.Send(updateCurrencyCommand);
                return Ok(isCUrrencyUpdate);
            }

            return BadRequest(updateCurrencyCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteCurrency = await Mediator.Send(new DeleteCurrencyCommand { Id = id });
            return Ok(isDeleteCurrency);
        }
    }
}