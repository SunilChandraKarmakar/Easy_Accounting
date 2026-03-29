namespace EasyAccountingAPI.Model.MasterSettings
{
    [Table("EnumTypes", Schema = "MasterSettings")]
    public class EnumType : IDelatableEntity
    {
        public EnumType()
        {
            EnumTypeCollections = new HashSet<EnumTypeCollection>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Please, provide name.")]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public ICollection<EnumTypeCollection> EnumTypeCollections { get; set; }
    }
}