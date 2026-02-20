namespace EasyAccountingAPI.Controllers.MasterSettings
{
    public class EmployeeController : BaseController
    {
        [HttpGet("{companyId}")]
        [ProducesResponseType(typeof(IEnumerable<SelectModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SelectModel>>> GetEmployeeByCompanyIdAsync(int companyId)
        {
            var employees = await Mediator.Send(new SelectListEmployeeByCompanyQuery { CompanyId = companyId });
            return Ok(employees);
        }
    }
}