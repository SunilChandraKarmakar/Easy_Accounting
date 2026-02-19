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

        [HttpDelete("{employeeId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(int employeeId)
        {
            var isDelete = await Mediator.Send(new DeleteEmployeeFeatureActionCommand { EmployeeId = employeeId });
            return Ok(isDelete);
        }
    }
}