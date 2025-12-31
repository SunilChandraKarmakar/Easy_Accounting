namespace EasyAccountingAPI.Database.DatabaseContexts
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions) { }

        #region Global Model
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        #endregion

        #region Authentication Model
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

                var connectionString = configuration.GetConnectionString("Default");
                optionsBuilder.UseSqlServer(connectionString);
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}