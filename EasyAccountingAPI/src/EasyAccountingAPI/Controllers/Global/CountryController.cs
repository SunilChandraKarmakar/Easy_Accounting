namespace EasyAccountingAPI.Controllers.Global
{
    public class CountryController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<CountryGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<CountryGridModel>>> GetAllAsync()
        {
            var getCountries = await Mediator.Send(new GetCountriesQuery());
            return Ok(getCountries);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<CountryGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<CountryGridModel>>> GetFilterCountriesAsync(
            GetCountriesByFilterQuery getCountriesByFilterQuery)
        {
            var getFilterCountries = await Mediator.Send(getCountriesByFilterQuery);
            return Ok(getFilterCountries);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CountryViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CountryViewModel>> GetByIdAsync(int id)
        {
            var countryVm = new CountryViewModel
            {
                UpdateModel = await Mediator.Send(new GetCountryDetailQuery { Id = id })
            };

            return Ok(countryVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateCountryCommand createCountryCommand)
        {
            if (ModelState.IsValid)
            {
                var isCountryCreated = await Mediator.Send(createCountryCommand);
                return Ok(isCountryCreated);
            }

            return BadRequest(createCountryCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(CountryUpdateModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(CountryUpdateModel countryUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var isCountryUpdate = await Mediator.Send(countryUpdateModel);
                return Ok(isCountryUpdate);
            }

            return BadRequest(countryUpdateModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteCountry = await Mediator.Send(new DeleteCountryCommand { Id = id });
            return Ok(isDeleteCountry);
        }
    }
}