namespace EasyAccountingAPI.Controllers.Purchase
{
    public class StorageLocationController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(FilterPageResultModel<StorageLocationGridModel>), StatusCodes.Status200OK)]
        [CheckAuthorize("StorageLocation", "List")]
        public async Task<ActionResult<FilterPageResultModel<StorageLocationGridModel>>> GetFilterStorageLocationsAsync(
            GetStorageLocationsByFilterQuery getStorageLocationsByFilterQuery)
        {
            var getFilterStorageLocations = await Mediator.Send(getStorageLocationsByFilterQuery);
            return Ok(getFilterStorageLocations);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StorageLocationViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<StorageLocationViewModel>> GetByIdAsync(string id)
        {
            var storageLocationVm = new StorageLocationViewModel
            {
                UpdateModel = await Mediator.Send(new GetStorageLocationDetailQuery { Id = id }),
            };

            // Select list
            storageLocationVm.OptionsDataSources.CompanySelectList = await Mediator.Send(new SelectListCompanyQuery());
            return Ok(storageLocationVm);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [CheckAuthorize("StorageLocation", "Create")]
        public async Task<ActionResult<bool>> CreateAsync(CreateStorageLocationCommand createStorageLocationCommand)
        {
            if (ModelState.IsValid)
            {
                var isStorageLocationCreated = await Mediator.Send(createStorageLocationCommand);
                return Ok(isStorageLocationCreated);
            }

            return BadRequest(createStorageLocationCommand);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [CheckAuthorize("StorageLocation", "Update")]
        public async Task<ActionResult<bool>> UpdateAsync(UpdateStorageLocationCommand updateStorageLocationCommand)
        {
            if (ModelState.IsValid)
            {
                var isStorageLocationUpdate = await Mediator.Send(updateStorageLocationCommand);
                return Ok(isStorageLocationUpdate);
            }

            return BadRequest(updateStorageLocationCommand);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [CheckAuthorize("StorageLocation", "Delete")]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var isDeleteStorageLocation = await Mediator.Send(new DeleteStorageLocationCommand { Id = id });
            return Ok(isDeleteStorageLocation);
        }
    }
}