using Hub.Entity;
using System.Collections.Generic;

namespace Hub.DataAccess.Repositories
{
    public interface IEntityRepository<T>
        where T : class, IEntity, new()
    {
        void CommitChanges();
        void DeleteOnCommit(T entity);
        T GetEntity(int key);
        IEnumerable<T> GetAll();
        int InsertOnCommit(T entity);
    }
}