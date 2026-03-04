namespace EasyAccountingAPI.Controllers.ProductService
{
    public class VariationController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<VariationGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<VariationGridModel>>> GetFilterVariationsAsync(
            GetVariationsByFilterQuery getVariationsByFilterQuery)
        {
            var getFilterVariations = await Mediator.Send(getVariationsByFilterQuery);
            return Ok(getFilterVariations);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VariationViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<VariationViewModel>> GetByIdAsync(string id)
        {
            var variationVm = new VariationViewModel
            {
                UpdateModel = await Mediator.Send(new GetVariationDetailQuery { Id = id }),
            };

            // Select list
            variationVm.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());
            return Ok(variationVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateVariationCommand createVariationCommand)
        {
            if (ModelState.IsValid)
            {
                var isVariationCreated = await Mediator.Send(createVariationCommand);
                return Ok(isVariationCreated);
            }

            return BadRequest(createVariationCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateVariationCommand updateVariationCommand)
        {
            if (ModelState.IsValid)
            {
                var isVariationUpdate = await Mediator.Send(updateVariationCommand);
                return Ok(isVariationUpdate);
            }

            return BadRequest(updateVariationCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteVariation = await Mediator.Send(new DeleteVariationCommand { Id = id });
            return Ok(isDeleteVariation);
        }
    }
}