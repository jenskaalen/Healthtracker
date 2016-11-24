using System.Collections.Generic;
using System.Linq;
using Dapper.Contrib.Extensions;
using healthtracker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace healthtracker.Repository
{
    public class LogDayRepository: ILogDayRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly HealthtrackerContext _context;
        private readonly SiteUser _user;

        public LogDayRepository(IConnectionFactory connectionFactory, HealthtrackerContext context)
        {
            _connectionFactory = connectionFactory;
            _context = context;
            _user = context.SiteUsers.Include(x => x.LogDays).First();
        }

        public List<LogDay> GetAll()
        {
            return _user.LogDays.ToList();
            //using (var conn = _connectionFactory.GetConnection())
            //{
            //    return conn.GetAll<LogDay>().ToList();
            //}
        }

        public LogDay Add(LogDay entity)
        {
            if (_user.LogDays == null)
                _user.LogDays = new List<LogDay>();

            foreach (var result in entity.LogEntries)
            {
                //_context.Attach(result).State = EntityState.Added;
                _context.Attach(result.LogType).State = EntityState.Unchanged;
            }
            
            _user.LogDays.Add(entity);
            _context.LogDays.Add(entity);
            
            _context.SaveChanges();
            return entity;
        }

        public LogDay Update(LogDay entity)
        {
            _context.LogDays.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public LogDay GetById(int id)
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                return conn.Get<LogDay>(id);
            }
        }

        public void Delete(int id)
        {
            var day = _context.LogDays.Find(id);
            _context.LogDays.Remove(day);
            _context.SaveChanges();
        }
    }
}
