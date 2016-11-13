using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using healthtracker.Model;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace healthtracker.Repository
{
    public class LogDayRepository: ILogDayRepository
    {
        public IEnumerable<LogDay> List { get; }

        public void Add(LogDay entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(LogDay entity)
        {
            throw new NotImplementedException();
        }

        public void Update(LogDay entity)
        {
            throw new NotImplementedException();
        }

        public LogDay GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
