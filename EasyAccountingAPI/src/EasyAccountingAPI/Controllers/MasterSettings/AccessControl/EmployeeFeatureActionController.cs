namespace EasyAccountingAPI.Controllers.MasterSettings.AccessControl
{
    public class EmployeeFeatureActionController : BaseController
    {
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
    }
}