namespace EasyAccountingAPI.Controllers.Global
{
    public class TestController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> GetAllAsync()
        {
            return Ok(true);
        }
    }
}