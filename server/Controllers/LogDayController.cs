using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using healthtracker.Model;
using healthtracker.Repository;
using Microsoft.AspNetCore.Mvc;

namespace healthtracker.Controllers
{
    [Route("api/[controller]")]
    public class LogDayController : BaseController<LogDay>
    {
        public LogDayController(IRepository<LogDay> repository) : base(repository)
        {
        }
    }
}
