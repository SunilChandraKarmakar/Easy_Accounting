namespace EasyAccountingAPI.Controllers.ProductService
{
    public class ProductController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<ProductGridModel>), StatusCodes.Status200OK)]
        [CheckAuthorize("Product", "List")]
        public async Task<ActionResult<FilterPageResultModel<ProductGridModel>>> GetFilterProductsAsync(
            GetProductsByFilterQuery getProductsByFilterQuery)
        {
            var getFilterProducts = await Mediator.Send(getProductsByFilterQuery);
            return Ok(getFilterProducts);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<ProductGridModel>), StatusCodes.Status200OK)]
        [CheckAuthorize("Product", "List")]
        public async Task<ActionResult<FilterPageResultModel<ProductGridModel>>> GetFilterExpiredProductsAsync(
            GetExpiredProductsByFilterQuery getExpiredProductsByFilterQuery)
        {
            var getFilterProducts = await Mediator.Send(getExpiredProductsByFilterQuery);
            return Ok(getFilterProducts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductViewModel>> GetByIdAsync(string id)
        {
            var productVm = new ProductViewModel
            {
                UpdateModel = await Mediator.Send(new GetProductDetailQuery { Id = id }),
            };

            // Select list
            productVm.OptionsDataSources.ProductUnitSelectList = await Mediator.Send(new SelectListProductUnitQuery());
            productVm.OptionsDataSources.ParentCategorySelectList = await Mediator.Send(new SelectListParentCategoryQuery());
            productVm.OptionsDataSources.BrandSelectList = await Mediator.Send(new SelectListBrandQuery());
            productVm.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());
            productVm.OptionsDataSources.VatTaxSelectList = await Mediator.Send(new SelectListVatTaxQuery());

            return Ok(productVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [CheckAuthorize("Product", "Create")]
        public async Task<ActionResult<bool>> CreateAsync(CreateProductCommand createProductCommand)
        {
            if (ModelState.IsValid)
            {
                var isProductCreated = await Mediator.Send(createProductCommand);
                return Ok(isProductCreated);
            }

            return BadRequest(createProductCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [CheckAuthorize("Product", "Update")]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateProductCommand updateProductCommand)
        {
            if (ModelState.IsValid)
            {
                var isProductUpdate = await Mediator.Send(updateProductCommand);
                return Ok(isProductUpdate);
            }

            return BadRequest(updateProductCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [CheckAuthorize("Product", "Delete")]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteProduct = await Mediator.Send(new DeleteProductCommand { Id = id });
            return Ok(isDeleteProduct);
        }
    }
}