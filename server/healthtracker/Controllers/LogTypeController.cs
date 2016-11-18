using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using healthtracker.Model;
using healthtracker.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace healthtracker.Controllers
{
    [Route("api/[controller]")]
    public class LogTypeController : BaseController<LogType>
    {
        public LogTypeController(IRepository<LogType> repository) : base(repository)
        {
        }
    }
}
