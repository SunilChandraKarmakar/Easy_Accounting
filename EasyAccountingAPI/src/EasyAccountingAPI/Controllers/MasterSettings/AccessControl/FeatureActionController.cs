namespace EasyAccountingAPI.Controllers.MasterSettings.AccessControl
{
    public class FeatureActionController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateFeatureActionCommand createFeatureActionCommand)
        {
            if (ModelState.IsValid)
            {
                var isFeatureActionCreated = await Mediator.Send(createFeatureActionCommand);
                return Ok(isFeatureActionCreated);
            }

            return BadRequest(createFeatureActionCommand);
        }
    }
}