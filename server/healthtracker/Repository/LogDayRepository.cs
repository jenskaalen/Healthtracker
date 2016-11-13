using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using healthtracker.Model;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace healthtracker.Repository
{
    public class LogDayRepository: ILogDayRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public LogDayRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<LogDay> GetAll()
        {
            using (var conn = new SqlConnection())
            {
                return conn.Query<LogDay>("").ToList();
            }
        }

        public LogDay Add(LogDay entity)
        {
            throw new NotImplementedException();
        }

        public LogDay Update(LogDay entity)
        {
            throw new NotImplementedException();
        }

        public LogDay GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
