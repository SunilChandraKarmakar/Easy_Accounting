namespace EasyAccountingAPI.Model.MasterSettings
{
    [Table("Currencies", Schema = "MasterSettings")]
    public class Currency : IDelatableEntity
    {
        public Currency()
        {
            Companies = new HashSet<Company>();
        }

        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Currency name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Base rate is required.")]
        public double BaseRate { get; set; }

        public string? Symble { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public ICollection<Company> Companies { get; set; }
    }
}