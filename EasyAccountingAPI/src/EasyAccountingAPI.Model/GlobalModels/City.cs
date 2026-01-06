namespace EasyAccountingAPI.Model.GlobalModels
{
    [Table("Cities", Schema = "Global")]
    public class City : IDelatableEntity
    {
        public City()
        {
            Companies = new HashSet<Company>();
        }

        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "City name is required.")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [ForeignKey("Country")]
        [Required(ErrorMessage = "CountryId is required.")]
        public int CountryId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public Country Country { get; set; }
        public ICollection<Company> Companies { get; set; }
    }
}