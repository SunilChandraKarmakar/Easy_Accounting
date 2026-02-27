namespace EasyAccountingAPI.Controllers.MasterSettings
{
    public class ProductUnitController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<ProductUnitGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<ProductUnitGridModel>>> GetFilterProductUnitsAsync(
            GetProductUnitsByFilterQuery getProductUnitsByFilterQuery)
        {
            var getFilterProductUnits = await Mediator.Send(getProductUnitsByFilterQuery);
            return Ok(getFilterProductUnits);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductUnitViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductUnitViewModel>> GetByIdAsync(string id)
        {
            var productUnitVm = new ProductUnitViewModel
            {
                UpdateModel = await Mediator.Send(new GetProductUnitDetailQuery { Id = id }),
            };

            return Ok(productUnitVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateProductUnitCommand createProductUnitCommand)
        {
            if (ModelState.IsValid)
            {
                var isProductUnitCreate = await Mediator.Send(createProductUnitCommand);
                return Ok(isProductUnitCreate);
            }

            return BadRequest(createProductUnitCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateProductUnitCommand updateProductUnitCommand)
        {
            if (ModelState.IsValid)
            {
                var isProductUnitUpdate = await Mediator.Send(updateProductUnitCommand);
                return Ok(isProductUnitUpdate);
            }

            return BadRequest(updateProductUnitCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteProductUnit = await Mediator.Send(new DeleteProductUnitCommand { Id = id });
            return Ok(isDeleteProductUnit);
        }
    }
}