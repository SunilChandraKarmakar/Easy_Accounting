namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CityLogic.Model
{
    public class CityViewModel
    {
        public CityCreateModel CreateModel { get; set; }
        public CityUpdateModel UpdateModel { get; set; }
        public CityGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class CityCreateModel : IMapFrom<City>
    {
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "City name is required.")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [ForeignKey("Country")]
        [Required(ErrorMessage = "CountryId is required.")]
        public int CountryId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<City, CityCreateModel>();
            profile.CreateMap<CityCreateModel, City>();
        }
    }

    public class CityUpdateModel : IMapFrom<City>
    {
        public int Id { get; set; }
        [NotMapped] public string? TempId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "City name is required.")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.", MinimumLength = 2)]
        public string Name { get; set; }

        public int? CountryId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<City, CityUpdateModel>();
            profile.CreateMap<CityUpdateModel, City>();
        }
    }

    public class CityGridModel : IMapFrom<City>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<City, CityGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.CountryName, s => s.MapFrom(m => m.Country.Name));
            profile.CreateMap<CityGridModel, City>();
        }
    }
}