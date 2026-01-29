namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.ActionLogic.Model
{
    public class ActionViewModel
    {
        public ActionCreateModel CreateModel { get; set; }
        public ActionUpdateModel UpdateModel { get; set; }
        public ActionGridModel GridModel { get; set; }  
    }

    public class ActionCreateModel : IMapFrom<EasyAccountingAPI.Model.MasterSettings.AccessControl.Action>
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ActionCreateModel, EasyAccountingAPI.Model.MasterSettings.AccessControl.Action>();
        }
    }

    public class ActionUpdateModel : IMapFrom<EasyAccountingAPI.Model.MasterSettings.AccessControl.Action>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ActionUpdateModel, EasyAccountingAPI.Model.MasterSettings.AccessControl.Action>();
            profile.CreateMap<EasyAccountingAPI.Model.MasterSettings.AccessControl.Action, ActionUpdateModel>();
        }
    }

    public class ActionGridModel : IMapFrom<EasyAccountingAPI.Model.MasterSettings.AccessControl.Action>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EasyAccountingAPI.Model.MasterSettings.AccessControl.Action, ActionGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())));
        }
    }
}