namespace EasyAccountingAPI.Controllers.ProductService
{
    public class BrandController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<BrandGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<BrandGridModel>>> GetFilterBrandsAsync(
            GetBrandsByFilterQuery getBrandsByFilterQuery)
        {
            var getFilterBrands = await Mediator.Send(getBrandsByFilterQuery);
            return Ok(getFilterBrands);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BrandViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<BrandViewModel>> GetByIdAsync(string id)
        {
            var brandVm = new BrandViewModel
            {
                UpdateModel = await Mediator.Send(new GetBrandDetailQuery { Id = id }),
            };

            // Select list
            brandVm.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());
            return Ok(brandVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateBrandCommand createBrandCommand)
        {
            if (ModelState.IsValid)
            {
                var isBrandCreated = await Mediator.Send(createBrandCommand);
                return Ok(isBrandCreated);
            }

            return BadRequest(createBrandCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateBrandCommand updateBrandCommand)
        {
            if (ModelState.IsValid)
            {
                var isBrandUpdate = await Mediator.Send(updateBrandCommand);
                return Ok(isBrandUpdate);
            }

            return BadRequest(updateBrandCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteBrand = await Mediator.Send(new DeleteBrandCommand { Id = id });
            return Ok(isDeleteBrand);
        }
    }
}