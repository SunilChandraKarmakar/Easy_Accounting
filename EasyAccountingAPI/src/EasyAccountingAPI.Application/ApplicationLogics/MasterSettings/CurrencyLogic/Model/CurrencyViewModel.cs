namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Model
{
    public class CurrencyViewModel
    {
        public CurrencyCreateModel CreateModel { get; set; }
        public CurrencyUpdateModel UpdateModel { get; set; }
        public CurrencyGridModel GridModel { get; set; }
    }

    public class CurrencyCreateModel : IMapFrom<Currency>
    {
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Currency name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Base rate is required.")]
        public double BaseRate { get; set; }

        public string? Symble { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Currency, CurrencyCreateModel>();
            profile.CreateMap<CurrencyCreateModel, Currency>();
        }
    }

    public class CurrencyUpdateModel : IMapFrom<Currency>
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Currency name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Base rate is required.")]
        public double BaseRate { get; set; }

        public string? Symble { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Currency, CurrencyUpdateModel>();
            profile.CreateMap<CurrencyUpdateModel, Currency>();
        }
    }

    public class CurrencyGridModel : IMapFrom<Currency>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double BaseRate { get; set; }
        public string? Symble { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Currency, CurrencyGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())));
            profile.CreateMap<CurrencyGridModel, Currency>();
        }
    }
}