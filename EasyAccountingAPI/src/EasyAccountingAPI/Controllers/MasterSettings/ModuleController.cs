namespace EasyAccountingAPI.Controllers.MasterSettings
{
    public class ModuleController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<ModuleGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<ModuleGridModel>>> GetFilterModulesAsync(
            GetModulesByFilterQuery getModulesByFilterQuery)
        {
            var getFilterModules = await Mediator.Send(getModulesByFilterQuery);
            return Ok(getFilterModules);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ModuleViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<ModuleViewModel>> GetByIdAsync(string id)
        {
            var moduleVm = new ModuleViewModel
            {
                UpdateModel = await Mediator.Send(new GetModuleDetailQuery { Id = id }),
            };

            return Ok(moduleVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateModuleCommand createModuleCommand)
        {
            if (ModelState.IsValid)
            {
                var isModuleCreated = await Mediator.Send(createModuleCommand);
                return Ok(isModuleCreated);
            }

            return BadRequest(createModuleCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateModuleCommand updateModuleCommand)
        {
            if (ModelState.IsValid)
            {
                var isModuleUpdate = await Mediator.Send(updateModuleCommand);
                return Ok(isModuleUpdate);
            }

            return BadRequest(updateModuleCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteModule = await Mediator.Send(new DeleteModuleCommand { Id = id });
            return Ok(isDeleteModule);
        }
    }
}