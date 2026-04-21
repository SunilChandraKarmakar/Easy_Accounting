namespace EasyAccountingAPI.Model.MasterSettings
{
    [Table("EnumTypeCollections", Schema = "MasterSettings")]
    public class EnumTypeCollection : IDelatableEntity
    {
        public EnumTypeCollection()
        {
            PurchasePaymentStatuses = new HashSet<Purchase.Purchase>();
            PurchasePaymentMethods = new HashSet<Purchase.Purchase>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Please, provide name.")]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please, selected enum type.")]
        public int EnumTypeId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public EnumType EnumType { get; set; }
        public ICollection<Purchase.Purchase> PurchasePaymentStatuses { get; set; }
        public ICollection<Purchase.Purchase> PurchasePaymentMethods { get; set; }
    }
}