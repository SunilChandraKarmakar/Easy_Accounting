namespace EasyAccountingAPI.Controllers.ProductService
{
    public class ProductController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateProductCommand createProductCommand)
        {
            if (ModelState.IsValid)
            {
                var isProductCreated = await Mediator.Send(createProductCommand);
                return Ok(isProductCreated);
            }

            return BadRequest(createProductCommand);
        }
    }
}