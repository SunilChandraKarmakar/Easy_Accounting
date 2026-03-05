namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.StorageLocationLogic.Model
{
    public class StorageLocationViewModel
    {
        public StorageLocationCreateModel CreateModel { get; set; } 
        public StorageLocationUpdateModel UpdateModel { get; set; }
        public StorageLocationGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class StorageLocationCreateModel : IMapFrom<StorageLocation>
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public string? Capacity { get; set; }
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StorageLocationCreateModel, StorageLocation>();
        }
    }

    public class StorageLocationUpdateModel : IMapFrom<StorageLocation>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public string? Capacity { get; set; }
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StorageLocation, StorageLocationUpdateModel>();
            profile.CreateMap<StorageLocationUpdateModel, StorageLocation>();
        }
    }

    public class StorageLocationGridModel : IMapFrom<StorageLocation>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string? Capacity { get; set; }
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StorageLocation, StorageLocationGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company.Name));
        }
    }
}