namespace EasyAccountingAPI.Controllers.MasterSettings.AccessControl
{
    public class EmployeeFeatureActionController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<EmployeeFeatureActionGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<EmployeeFeatureActionGridModel>>> GetFilterEmployeeFeatureActionsAsync(GetEmloyeeFeatureActionsByFilterQuery emloyeeFeatureActionsQuery)
        {
            var getEmloyeeFeatureActions = await Mediator.Send(emloyeeFeatureActionsQuery);
            return Ok(getEmloyeeFeatureActions);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateEmployeeFeatureActionCommand createEmployeeFeatureActionCommand)
        {
            if (ModelState.IsValid)
            {
                var isEmployeeFeatureActionCreated = await Mediator.Send(createEmployeeFeatureActionCommand);
                return Ok(isEmployeeFeatureActionCreated);
            }

            return BadRequest(createEmployeeFeatureActionCommand);
        }

        [HttpGet("{employeeId}")]
        [ProducesResponseType(typeof(EmployeeFeatureActionViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<EmployeeFeatureActionViewModel>> GetByIdAsync(string employeeId)
        {
            var employeeFeatureActionVM = new EmployeeFeatureActionViewModel
            {
                UpdateModel = await Mediator.Send(new GetEmployeeFeatureActionDetailQuery { EmployeeId = employeeId }),
            };

            // Get select list
            employeeFeatureActionVM.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());
            employeeFeatureActionVM.OptionsDataSources.ActionSelectList = await Mediator.Send(new SelectListActionQuery());
            employeeFeatureActionVM.OptionsDataSources.ModuleSelectList = await Mediator.Send(new SelectListModuleQuery());
            employeeFeatureActionVM.OptionsDataSources.FeatureSelectList = await Mediator.Send(new SelectListFeatureQuery());
            return Ok(employeeFeatureActionVM);
        }

        [HttpDelete("{employeeId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(int employeeId)
        {
            var isDelete = await Mediator.Send(new DeleteEmployeeFeatureActionCommand { EmployeeId = employeeId });
            return Ok(isDelete);
        }
    }
}