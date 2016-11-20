using System.Collections.Generic;

namespace healthtracker.Repository
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T Add(T entity);
        T Update(T entity);
        T GetById(int id);
        void Delete(int id);
    }
}