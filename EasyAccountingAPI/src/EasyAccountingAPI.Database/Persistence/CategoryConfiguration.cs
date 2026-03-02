namespace EasyAccountingAPI.Database.Persistence
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(er => er.Id);

            builder.HasOne(er => er.ParentCategory)
                   .WithMany(e => e.SubCategories)
                   .HasForeignKey(er => er.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}