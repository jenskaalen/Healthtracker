using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace healthtracker.Model
{
    public class HealthtrackerContext: DbContext
    {
        public HealthtrackerContext(DbContextOptions<HealthtrackerContext> options)
            : base(options)
        {
        }

        public DbSet<LogDay> LogDays { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<LogType> LogTypes { get; set; }
        public DbSet<SiteUser> SiteUsers { get; set; }
    }
}
