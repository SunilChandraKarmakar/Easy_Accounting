namespace EasyAccountingAPI.Controllers.ProductService
{
    public class CategoryController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<CategoryGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<CategoryGridModel>>> GetFilterCategoriesAsync(
            GetCategoriesByFilterQuery getCategoriesByFilterQuery)
        {
            var getFilterCategories = await Mediator.Send(getCategoriesByFilterQuery);
            return Ok(getFilterCategories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryViewModel>> GetByIdAsync(string id)
        {
            var categoryVm = new CategoryViewModel
            {
                UpdateModel = await Mediator.Send(new GetCategoryDetailQuery { Id = id }),
            };

            // Select list
            categoryVm.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());
            categoryVm.OptionsDataSources.ParentCategorySelectList = await Mediator.Send(new SelectListParentCategoryQuery());

            return Ok(categoryVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateCategoryCommand createCategoryCommand)
        {
            if (ModelState.IsValid)
            {
                var isCategoryCreated = await Mediator.Send(createCategoryCommand);
                return Ok(isCategoryCreated);
            }

            return BadRequest(createCategoryCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateCategoryCommand updateCategoryCommand)
        {
            if (ModelState.IsValid)
            {
                var isCategoryUpdate = await Mediator.Send(updateCategoryCommand);
                return Ok(isCategoryUpdate);
            }

            return BadRequest(updateCategoryCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteCategory = await Mediator.Send(new DeleteCategoryCommand { Id = id });
            return Ok(isDeleteCategory);
        }

        [HttpGet("{parentId}")]
        [ProducesResponseType(typeof(IEnumerable<SelectModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SelectModel>>> GetCategoriesByParentIdAsync(int parentId)
        {
            var categories = await Mediator.Send(new SelectListCategoryByParentIdQuery { ParentId = parentId });
            return Ok(categories);
        }
    }
}