namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ModuleLogic.Model
{
    public class ModuleViewModel
    {
        public ModuleCreateModel CreateModel { get; set; }
        public ModuleUpdateModel UpdateModel { get; set; }
        public ModuleGridModel GridModel { get; set; }  
    }

    public class ModuleCreateModel : IMapFrom<EasyAccountingAPI.Model.MasterSettings.Module>
    {
        [Required(ErrorMessage = "Module name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Module name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ModuleCreateModel, EasyAccountingAPI.Model.MasterSettings.Module>();
        }
    }

    public class ModuleUpdateModel : IMapFrom<EasyAccountingAPI.Model.MasterSettings.Module>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Module name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Module name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ModuleUpdateModel, EasyAccountingAPI.Model.MasterSettings.Module>();
            profile.CreateMap<EasyAccountingAPI.Model.MasterSettings.Module, ModuleUpdateModel>();
        }
    }

    public class ModuleGridModel : IMapFrom<EasyAccountingAPI.Model.MasterSettings.Module>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ModuleUpdateModel, EasyAccountingAPI.Model.MasterSettings.Module>();
            profile.CreateMap<EasyAccountingAPI.Model.MasterSettings.Module, ModuleGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())));
        }
    }
}