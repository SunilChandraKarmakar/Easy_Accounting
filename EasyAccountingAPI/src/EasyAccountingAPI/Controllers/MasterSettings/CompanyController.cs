namespace EasyAccountingAPI.Controllers.MasterSettings
{
    public class CompanyController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<CompanyGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<CompanyGridModel>>> GetFilterCompaniesAsync(
            GetCompaniesByFilterQuery getCompaniesByFilterQuery)
        {
            var getFilterCompanies = await Mediator.Send(getCompaniesByFilterQuery);
            return Ok(getFilterCompanies);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CompanyViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyViewModel>> GetByIdAsync(string id)
        {
            var companyVm = new CompanyViewModel
            {
               UpdateModel = await Mediator.Send(new GetCompanyDetailQuery { Id = id }),
            };

            // Get select list
            companyVm.OptionsDataSources.CountrySelectList = await Mediator.Send(new SelectListCountryQuery());
            companyVm.OptionsDataSources.CitySelectList = await Mediator.Send(new SelectListCityQuery());
            companyVm.OptionsDataSources.CurrencySelectList = await Mediator.Send(new SelectListCurrencyQuery());

            return Ok(companyVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CompanyCreateCommand createCompanyCommand)
        {
            if (ModelState.IsValid)
            {
                var isCompanyCreated = await Mediator.Send(createCompanyCommand);
                return Ok(isCompanyCreated);
            }

            return BadRequest(createCompanyCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(CompanyUpdateCommand updateCompanyCommand)
        {
            if (ModelState.IsValid)
            {
                var isCompanyUpdate = await Mediator.Send(updateCompanyCommand);
                return Ok(isCompanyUpdate);
            }

            return BadRequest(updateCompanyCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteCompany = await Mediator.Send(new DeleteCompanyCommand { Id = id });
            return Ok(isDeleteCompany);
        }
    }
}