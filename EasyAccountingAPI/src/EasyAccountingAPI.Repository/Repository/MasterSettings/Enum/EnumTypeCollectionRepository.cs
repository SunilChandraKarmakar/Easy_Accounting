namespace EasyAccountingAPI.Repository.Repository.MasterSettings.Enum
{
    public class EnumTypeCollectionRepository : BaseRepository<EnumTypeCollection>, IEnumTypeCollectionRepository
    {
        public EnumTypeCollectionRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<ICollection<EnumTypeCollection>> GetAllAsync(CancellationToken cancellationToken)
        {
            var enumTypeCollections = db.EnumTypeCollections
                .Where(etc => !etc.IsDeleted);

            return await enumTypeCollections.ToListAsync(cancellationToken);
        }
    }
}