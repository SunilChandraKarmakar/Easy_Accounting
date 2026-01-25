namespace EasyAccountingAPI.Model.MasterSettings
{
    [Table("Modules", Schema = "MasterSettings")]
    public class Module : IDelatableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

    }
}