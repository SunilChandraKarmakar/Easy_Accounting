namespace EasyAccountingAPI.Repository.Repository.MasterSettings.Enum
{
    public class EnumTypeRepository : BaseRepository<EnumType>, IEnumTypeRepository
    {
        public EnumTypeRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<ICollection<EnumType>> GetAllAsync(CancellationToken cancellationToken)
        {
            var enumTypes = db.EnumTypes
                .Where(et => !et.IsDeleted);

            return await enumTypes.ToListAsync(cancellationToken);
        }
    }
}