using CzechUp.EF;
using CzechUp.EF.Models.Absract;

namespace CzechUp.Services
{
    public abstract class BaseService<DbEntity>
        where DbEntity : class, IDbEntity
    {
        private readonly DatabaseContext databaseContext;

        public BaseService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public virtual Guid Create(DbEntity entity)
        {
            databaseContext.Set<DbEntity>().Add(entity);
            databaseContext.SaveChanges();

            return entity.Guid;
        }

        public virtual void Update(DbEntity entity)
        {
            databaseContext.Set<DbEntity>().Update(entity);
            databaseContext.SaveChanges();
        }

        public virtual void Delete(Guid guid) 
        {
            var entity = databaseContext.Set<DbEntity>().Where(e => e.Guid == guid).FirstOrDefault();
            databaseContext.Remove(entity);
            databaseContext.SaveChanges();
        }

        public virtual DbEntity GetByGuid(Guid guid)
        {
            return databaseContext.Set<DbEntity>().FirstOrDefault(e => e.Guid == guid);
        }

        public virtual IQueryable<DbEntity> GetQuery()
        {
            return databaseContext.Set<DbEntity>().AsQueryable();
        }
    }
}
