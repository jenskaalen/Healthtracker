using System.Collections.Generic;
using System.Linq;
using healthtracker.Model;
using Microsoft.EntityFrameworkCore;

namespace healthtracker.Repository
{
    public class BaseRepository<T> : IRepository<T> where T: class
    {
        private readonly HealthtrackerContext _context;

        public BaseRepository(HealthtrackerContext context)
        {
            _context = context;
        }

        public virtual List<T> GetAll()
        {
            DbSet<T> all = _context.Set<T>();
            return all.ToList();
        }

        public virtual T Add(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual T Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual T GetById(int id)
        {
            var entity = _context.Find<T>(id);
            return entity;
        }

        public virtual void Delete(int id)
        {
            var entity = _context.Find<T>(id);
            _context.Remove(entity);
            _context.SaveChanges();
        }
    }
}