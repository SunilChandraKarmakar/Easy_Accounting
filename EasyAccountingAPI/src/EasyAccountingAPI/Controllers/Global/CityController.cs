namespace EasyAccountingAPI.Controllers.Global
{
    public class CityController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<CityGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<CityGridModel>>> GetFilterCitiesAsync(
            GetCityByFilterQuery getCitiesByFilterQuery)
        {
            var getFilterCities = await Mediator.Send(getCitiesByFilterQuery);
            return Ok(getFilterCities);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteCity = await Mediator.Send(new DeleteCityCommand { Id = id });
            return Ok(isDeleteCity);
        }
    }
}