namespace EasyAccountingAPI.Model.MasterSettings
{
    [Table("Modules", Schema = "MasterSettings")]
    public class Module : IDelatableEntity
    {
        public Module()
        {
            Features = new HashSet<AccessControl.Feature>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public ICollection<AccessControl.Feature> Features { get; set; }
    }
}