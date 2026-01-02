namespace EasyAccountingAPI.Database.Persistence
{
    public class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
    {
        public void Configure(EntityTypeBuilder<EmployeeRole> builder)
        {
            builder.HasKey(er => er.Id);

            builder.HasOne(er => er.Employee)
                   .WithMany(e => e.EmployeeRoles)
                   .HasForeignKey(er => er.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(er => er.AssignedByEmployee)
                   .WithMany(e => e.AssignedByEmployeeRoles)
                   .HasForeignKey(er => er.AssignedByEmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(er => er.Role)
                   .WithMany(r => r.EmployeeRoles)
                   .HasForeignKey(er => er.RoleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(er => new { er.EmployeeId, er.RoleId })
                   .IsUnique();
        }
    }
}