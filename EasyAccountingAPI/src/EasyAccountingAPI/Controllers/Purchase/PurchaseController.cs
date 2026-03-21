namespace EasyAccountingAPI.Controllers.Purchase
{
    public class PurchaseController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<PurchaseGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<PurchaseGridModel>>> GetFilterPurchasesAsync(
            GetPurchasesByFilterQuery getPurchasesByFilterQuery)
        {
            var getFilterPurchases = await Mediator.Send(getPurchasesByFilterQuery);
            return Ok(getFilterPurchases);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PurchaseViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<PurchaseViewModel>> GetByIdAsync(string id)
        {
            var purchaseVm = new PurchaseViewModel
            {
                UpdateModel = await Mediator.Send(new GetPurchaseDetailQuery { Id = id }),
            };

            // Select list
            purchaseVm.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());
            purchaseVm.OptionsDataSources.ProductSelectList = await Mediator.Send(new SelectListProductQuery());

            return Ok(purchaseVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreatePurchaseCommand createPurchaseCommand)
        {
            if (ModelState.IsValid)
            {
                var isPurchaseCreated = await Mediator.Send(createPurchaseCommand);
                return Ok(isPurchaseCreated);
            }

            return BadRequest(createPurchaseCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdatePurchaseCommand updatePurchaseCommand)
        {
            if (ModelState.IsValid)
            {
                var isPurchaseUpdate = await Mediator.Send(updatePurchaseCommand);
                return Ok(isPurchaseUpdate);
            }

            return BadRequest(updatePurchaseCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeletePurchase = await Mediator.Send(new DeletePurchaseCommand { Id = id });
            return Ok(isDeletePurchase);
        }
    }
}