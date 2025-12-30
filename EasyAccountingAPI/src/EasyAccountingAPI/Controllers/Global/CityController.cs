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

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateCityCommand createCityCommand)
        {
            if (ModelState.IsValid)
            {
                var isCityCreated = await Mediator.Send(createCityCommand);
                return Ok(isCityCreated);
            }

            return BadRequest(createCityCommand);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CityViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CityViewModel>> GetByIdAsync(string id)
        {
            var cityVM = new CityViewModel
            {
                UpdateModel = await Mediator.Send(new GetCityDetailQuery { Id = id })
            };

            // Select list for drop downs
            cityVM.OptionsDataSources.CountrySelectList = await Mediator.Send(new SelectListCountryQuery());
            cityVM.OptionsDataSources.CitySelectList = await Mediator.Send(new SelectListCityQuery());
            return Ok(cityVM);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateCityCommand updateCityCommand)
        {
            if (ModelState.IsValid)
            {
                var isCityUpdate = await Mediator.Send(updateCityCommand);
                return Ok(isCityUpdate);
            }

            return BadRequest(updateCityCommand);
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