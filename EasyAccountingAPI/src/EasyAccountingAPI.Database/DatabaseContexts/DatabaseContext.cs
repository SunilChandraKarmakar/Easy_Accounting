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
        public DbSet<UserLoginHistory> UserLoginHistories { get; set; }
        #endregion

        #region Master Settings Model
        public DbSet<Company> Companies { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InvoiceSetting> InvoiceSettings { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Model.MasterSettings.AccessControl.Action> Actions { get; set; }
        public DbSet<Model.MasterSettings.AccessControl.Feature> Features { get; set; }
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
            // Global delete behavior rule
            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            // Apply all configurations automatically
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(DatabaseContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}