using System.Linq;

namespace SecretSanta.Data.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(object id);

        IQueryable<T> All { get; }

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
