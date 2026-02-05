namespace EasyAccountingAPI.Controllers.MasterSettings.AccessControl
{
    public class FeatureController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<FeatureGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<FeatureGridModel>>> GetFilterFeaturesAsync(
            GetFeaturesByFilterQuery getFeaturesByFilterQuery)
        {
            var getFilterFeatures = await Mediator.Send(getFeaturesByFilterQuery);
            return Ok(getFilterFeatures);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FeatureViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<FeatureViewModel>> GetByIdAsync(string id)
        {
            var featureVm = new FeatureViewModel
            {
                UpdateModel = await Mediator.Send(new GetFeatureDetailQuery { Id = id }),
            };

            // Get select list
            featureVm.OptionsDataSources.ModuleSelectList = await Mediator.Send(new SelectListModuleQuery());
            return Ok(featureVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateFeatureCommand createFeatureCommand)
        {
            if (ModelState.IsValid)
            {
                var isFeatureCreated = await Mediator.Send(createFeatureCommand);
                return Ok(isFeatureCreated);
            }

            return BadRequest(createFeatureCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateFeatureCommand updateFeatureCommand)
        {
            if (ModelState.IsValid)
            {
                var isFeatureUpdate = await Mediator.Send(updateFeatureCommand);
                return Ok(isFeatureUpdate);
            }

            return BadRequest(updateFeatureCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteFeature = await Mediator.Send(new DeleteFeatureCommand { Id = id });
            return Ok(isDeleteFeature);
        }

        // Get features by module id where feature id is not use in the feature action table
        [HttpGet("moduleId")]
        [ProducesResponseType(typeof(IEnumerable<SelectModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SelectModel>>> GetFeatureByModuleIdAsync(int moduleId)
        {
            var getFeatures = await Mediator.Send(new SelectListFeatureByModuleIdQuery { ModuleId = moduleId });
            return Ok(getFeatures);
        }
    }
}