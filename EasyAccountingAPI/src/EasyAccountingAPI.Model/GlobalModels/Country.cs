namespace EasyAccountingAPI.Model.GlobalModels
{
    [Table("Countries", Schema = "Global")]
    public class Country : IDelatableEntity
    {
        public Country()
        {
            Cities = new HashSet<City>();
            Companies = new HashSet<Company>();
        }

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
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public ICollection<City> Cities { get; set; } 
        public ICollection<Company> Companies { get; set; }
    }
}