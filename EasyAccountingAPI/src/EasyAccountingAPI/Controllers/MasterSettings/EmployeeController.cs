namespace EasyAccountingAPI.Controllers.MasterSettings
{
    public class EmployeeController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<EmployeeGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<EmployeeGridModel>>> GetFilterEmployeesAsync(
            GetEmployeeByFilterQuery getEmployeesByFilterQuery)
        {
            var getFilterEmployees = await Mediator.Send(getEmployeesByFilterQuery);
            return Ok(getFilterEmployees);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<EmployeeViewModel>> GetByIdAsync(string id)
        {
            var employeeVm = new EmployeeViewModel
            {
                UpdateModel = await Mediator.Send(new GetEmployeeDetailQuery { Id = id }),
            };

            // Set select list
            employeeVm.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());
            return Ok(employeeVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateEmployeeCommand createEmployeeCommand)
        {
            if (ModelState.IsValid)
            {
                var isEmployeeCreate = await Mediator.Send(createEmployeeCommand);
                return Ok(isEmployeeCreate);
            }

            return BadRequest(createEmployeeCommand);
        }

        [HttpGet("{companyId}")]
        [ProducesResponseType(typeof(IEnumerable<SelectModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SelectModel>>> GetEmployeeByCompanyIdAsync(int companyId)
        {
            var employees = await Mediator.Send(new SelectListEmployeeByCompanyQuery { CompanyId = companyId });
            return Ok(employees);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateEmployeeCommand updateEmployeeCommand)
        {
            if (ModelState.IsValid)
            {
                var isEmployeeUpdate = await Mediator.Send(updateEmployeeCommand);
                return Ok(isEmployeeUpdate);
            }

            return BadRequest(updateEmployeeCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteEmployee = await Mediator.Send(new DeleteEmployeeCommand { Id = id });
            return Ok(isDeleteEmployee);
        }
    }
}