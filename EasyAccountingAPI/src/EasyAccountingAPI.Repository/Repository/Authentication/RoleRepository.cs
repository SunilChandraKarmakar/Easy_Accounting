namespace EasyAccountingAPI.Repository.Repository.Authentication
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}