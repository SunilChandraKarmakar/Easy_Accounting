namespace EasyAccountingAPI.Model.GlobalModels
{
    [Table("Cities", Schema = "Global")]
    [Index(nameof(Name), IsUnique = true)]
    public class City : IDelatableEntity
    {
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

        public virtual Country Country { get; set; }
    }
}