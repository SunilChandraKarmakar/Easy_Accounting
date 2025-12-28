namespace EasyAccountingAPI.Controllers.Global
{
    public class CityController : BaseController
    {
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            var isDeleteCity = await Mediator.Send(new DeleteCityCommand { Id = id });
            return Ok(isDeleteCity);
        }
    }
}