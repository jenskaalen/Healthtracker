using System.Collections.Generic;

namespace healthtracker.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> List { get; }
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(int id);
    }
}