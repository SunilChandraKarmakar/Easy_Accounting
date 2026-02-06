namespace EasyAccountingAPI.Controllers.MasterSettings.AccessControl
{
    public class FeatureActionController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<FeatureActionGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<FeatureActionGridModel>>> GetFilterFeatureActionsAsync(
            GetFeatureActionsByFilterQuery getFeatureActionsByFilterQuery)
        {
            var getFeatureFilterActions = await Mediator.Send(getFeatureActionsByFilterQuery);
            return Ok(getFeatureFilterActions);
        }

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

        [HttpGet("{featureId}")]
        [ProducesResponseType(typeof(FeatureActionViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<FeatureActionViewModel>> GetByIdAsync(string featureId)
        {
            var featureActionVm = new FeatureActionViewModel
            {
                UpdateModel = await Mediator.Send(new GetFeatureActionDetailQuery { FeatureId = featureId }),
            };

            // Get select list
            featureActionVm.OptionsDataSources.ActionSelectList = await Mediator.Send(new SelectListActionQuery());
            featureActionVm.OptionsDataSources.ModuleSelectList = await Mediator.Send(new SelectListModuleQuery());
            return Ok(featureActionVm);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateFeatureActionCommand updateFeatureActionCommand)
        {
            if (ModelState.IsValid)
            {
                var isFeatureActionUpdate = await Mediator.Send(updateFeatureActionCommand);
                return Ok(isFeatureActionUpdate);
            }

            return BadRequest(updateFeatureActionCommand);
        }

        [HttpDelete("{featureId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(int featureId)
        {
            var isDeleteFeatureAction = await Mediator.Send(new DeleteFeatureActionCommand { FeatureId = featureId });
            return Ok(isDeleteFeatureAction);
        }
    }
}