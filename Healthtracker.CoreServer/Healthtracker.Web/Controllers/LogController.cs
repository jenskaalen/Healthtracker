using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Healthtracker.Web.Data;
using Healthtracker.Web.Model;
using Healthtracker.Web.Repositories;
using LiteDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Healthtracker.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogRepository _logRepository;
        private readonly int _logsPerPage = 10;

        private string UserId => User.Identity.Name;

        public LogController(IHostingEnvironment hostingEnvironment, ILogRepository logRepository)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._logRepository = logRepository;
        }

        // GET: api/Log
        [HttpGet]
        public IEnumerable<Log> Get()
        {
            string or = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            return _logRepository.GetAll(UserId);
        }

        // GET: api/Log/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        // GET: api/Log/5
        [HttpGet("query/{text}")]
        public List<Log> Search(string text)
        {
            return _logRepository.Search(text, UserId);
        }

        [HttpGet("page/{index}")]
        public List<Log> LogPage(int index)
        {
            //assuming first page is 1
            int logsToSkip = Math.Clamp((index - 1) * _logsPerPage, 0, int.MaxValue);
            var logs = _logRepository.Get(_logsPerPage, logsToSkip, UserId);
            return logs;
        }

        // POST: api/Log
        [HttpPost]
        public Log Post([FromBody] Log log)
        {
            log.UserId = UserId;
            return _logRepository.Update(log);
        }

        // PUT: api/Log/5
        [HttpPut("{id}")]
        public Log Put(int id, [FromBody] Log log)
        {
            log.UserId = UserId;
            return _logRepository.Update(log);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logRepository.Delete(id);
        }
    }
}
