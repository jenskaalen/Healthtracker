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
            return _logRepository.GetAll(UserId);
            ////TODO: get root path
            //using (var db = new LiteDatabase(_hostingEnvironment.WebRootPath + @"\..\Data\logdata.db"))
            //{
            //    string userId = User.Identity.Name;
            //    var col = db.GetCollection<Log>("logs");
            //    return col.Find(x => x.UserId == userId).ToList();
            //    //return col.FindAll();
            //}
        }

        // GET: api/Log/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("page/{index}")]
        public List<Log> LogPage(int index)
        {
            //assuming first page is 1
            int logsToSkip = Math.Clamp((index - 1) * _logsPerPage, 0, int.MaxValue);
            return _logRepository.Get(_logsPerPage, logsToSkip, UserId);
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
            //TODO: theres no real update handling in ravendb
            _logRepository.Update(log);
            return log;

            //TODO: get root path
            //using (var db = new LiteDatabase(_hostingEnvironment.WebRootPath + @"\..\Data\logdata.db"))
            //{
            //    string userId = User.Identity.Name;
            //    var col = db.GetCollection<Log>("logs");
            //    var existingLog = col.Find(q => q.Id == id && q.UserId == userId).FirstOrDefault();

            //    if (existingLog == null)
            //        throw new UnauthorizedAccessException();

            //    log.UserId = userId;

            //    bool updated = col.Update(log);

            //    if (!updated)
            //        throw new Exception("Didnt find log to update");
            //}
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logRepository.Delete(id);
        }
    }
}
