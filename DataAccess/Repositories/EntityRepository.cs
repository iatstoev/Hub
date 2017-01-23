using Hub.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hub.DataAccess.Repositories
{
    public class EntityRepository<T>
         : IEntityRepository<T>
         where T : class, IEntity, new()
    {
        private HubDbContext _entities;

        public EntityRepository(HubDbContext entities)
        {
            _entities = entities;
        }

        public void CommitChanges()
        {
            _entities.SaveChanges();
        }

        public void DeleteOnCommit(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        public T GetEntity(int key)
        {
            return _entities.Set<T>().Find(key);
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.Set<T>();
        }

        public int InsertOnCommit(T entity)
        {
            _entities.Set<T>().Add(entity);

            return entity.ID;
        }

        internal IQueryable<T> GetAllQueryable()
        {
            return _entities.Set<T>();
        }

        /// <summary>
        /// DO NOT USE THIS! :-)
        /// </summary>
        /// <param name="query"></param>
        internal void RunSqlQuery(string query)
        {
            _entities.Database.ExecuteSqlCommand(query);
        }

        //public T GetEntity<U>(int key, Expression<Func<T, U>> columns)
        //{
        //    if (columns == null)
        //        throw new Exception("You can use this method only with columns");

        //    IEnumerable<U> result = _entities.Set<T>().Where(el => el.ID == key).Select<T, U>(columns);
        //    if (result.Count() > 0)
        //        return (T)result.ElementAt(0).ToType<T>();
        //    return default(T);
        //}
    }







    public static class Helpers
    {
        //public static IQueryable<TSource> Include<TSource,TProperty> (this IQueryable<TSource> source, Expression<Func<TSource,TProperty>> path)
        //{
        //    var objectQuery = source as ObjectQuery<TSource>;
        //    if (objectQuery != null)
        //    {
        //        return objectQuery.Include(path);
        //    }
        //    return source;
        //}


        public static object ToType<T>(this object obj)
        {
            //create instance of T type object:
            //object tmp = Activator.CreateInstance(type);

            object tmp = Activator.CreateInstance<T>();

            //loop through the properties of the object you want to covert:          
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                //get the value of property and try to assign it to the property of T type object:
                var prop = tmp.GetType().GetProperty(pi.Name);
                if (prop != null)
                    prop.SetValue(tmp, pi.GetValue(obj, null), null);

            }
            //return the T type object:         
            return tmp;
        }
    }
}