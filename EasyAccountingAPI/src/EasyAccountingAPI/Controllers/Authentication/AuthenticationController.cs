namespace EasyAccountingAPI.Controllers.Authentication
{
    [AllowAnonymous]
    public class AuthenticationController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UserModel>> Registration(RegistrationCommand command)
        {
            if(ModelState.IsValid)
            {
                var registerUser = await Mediator.Send(command);
                return Ok(registerUser);
            }

            return BadRequest(command);
        }
    }
}