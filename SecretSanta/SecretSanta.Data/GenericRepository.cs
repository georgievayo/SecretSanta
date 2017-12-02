using System;
using System.Linq;
using SecretSanta.Data.Interfaces;

namespace SecretSanta.Data
{
    public class GenericRepository<T>: IRepository<T>
        where T : class
    {
        private readonly SecretSantaDbContext _dbContext;

        public GenericRepository(SecretSantaDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IQueryable<T> All
        {
            get
            {
                return this._dbContext.DbSet<T>();
            }
        }

        public void Add(T entity)
        {
            this._dbContext.SetAdded(entity);
        }

        public void Delete(T entity)
        {
            this._dbContext.SetDeleted(entity);
        }

        public T GetById(object id)
        {
            return this._dbContext.DbSet<T>().Find(id);
        }

        public void Update(T entity)
        {
            this._dbContext.SetUpdated(entity);
        }
    }
}
