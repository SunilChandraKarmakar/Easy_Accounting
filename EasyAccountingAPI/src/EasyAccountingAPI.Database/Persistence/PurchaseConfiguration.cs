namespace EasyAccountingAPI.Database.Persistence
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(er => er.Id);

            builder.HasOne(er => er.PaymentStatus)
                   .WithMany(e => e.PurchasePaymentStatuses)
                   .HasForeignKey(er => er.PaymentStatusId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(er => er.PaymentMethod)
                   .WithMany(e => e.PurchasePaymentMethods)
                   .HasForeignKey(er => er.PaymentMethodId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}