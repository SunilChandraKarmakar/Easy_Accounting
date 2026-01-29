using EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.ActionLogic.Command;
using EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.ActionLogic.Model;
using EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.ActionLogic.Queries;

namespace EasyAccountingAPI.Controllers.MasterSettings.AccessControl
{
    public class ActionController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<ActionGridModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<FilterPageResultModel<ActionGridModel>>> GetFilterActionsAsync(
            GetActionsByFilterQuery getActionsByFilterQuery)
        {
            var getFilterActions = await Mediator.Send(getActionsByFilterQuery);
            return Ok(getFilterActions);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActionViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<ActionViewModel>> GetByIdAsync(string id)
        {
            var actionVm = new ActionViewModel
            {
                UpdateModel = await Mediator.Send(new GetActionDetailQuery { Id = id })
            };

            return Ok(actionVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateAsync(CreateActionCommand createActionCommand)
        {
            if (ModelState.IsValid)
            {
                var isActionCreated = await Mediator.Send(createActionCommand);
                return Ok(isActionCreated);
            }

            return BadRequest(createActionCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateActionCommand updateActionCommand)
        {
            if (ModelState.IsValid)
            {
                var isActionUpdate = await Mediator.Send(updateActionCommand);
                return Ok(isActionUpdate);
            }

            return BadRequest(updateActionCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteAction = await Mediator.Send(new DeleteActionCommand { Id = id });
            return Ok(isDeleteAction);
        }
    }
}