namespace EasyAccountingAPI.Database.Persistence
{
    public class VendorAddressConfiguration : IEntityTypeConfiguration<VendorAddress>
    {
        public void Configure(EntityTypeBuilder<VendorAddress> builder)
        {
            builder.HasKey(er => er.Id);

            builder.HasOne(er => er.Vendor)
                   .WithMany(e => e.VendorAddresses)
                   .HasForeignKey(er => er.VendorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}