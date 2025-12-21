namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Model
{
    public class CountryViewModel
    {
        public CountryCreateModel CreateModel { get; set; }
        public CountryUpdateModel UpdateModel { get; set; }
        public CountryGridModel GridModel { get; set; }
    }

    public class CountryCreateModel : IMapFrom<Country>
    {
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Country name is required.")]
        [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Required(ErrorMessage = "Country code is required.")]
        [StringLength(10, ErrorMessage = "Country code cannot exceed 10 characters.", MinimumLength = 1)]
        public string Code { get; set; }

        public string? Icon { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Country, CountryCreateModel>();
            profile.CreateMap<CountryCreateModel, Country>();
        }
    }

    public class CountryUpdateModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Country name is required.")]
        [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Required(ErrorMessage = "Country code is required.")]
        [StringLength(10, ErrorMessage = "Country code cannot exceed 10 characters.", MinimumLength = 1)]
        public string Code { get; set; }

        public string? Icon { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Country, CountryUpdateModel>();
            profile.CreateMap<CountryUpdateModel, Country>();            
        }
    }

    public class CountryGridModel : IMapFrom<Country>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Icon { get; set; }

        public ICollection<CityGridModel> Cities { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Country, CountryGridModel>()
                .ForMember(d => d.Cities, s => s.MapFrom(m => m.Cities));
            profile.CreateMap<CountryGridModel, Country>(); 
        }
    }
}