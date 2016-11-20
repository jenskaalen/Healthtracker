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
    public class LogDayController : Controller
    {
        private readonly ILogDayRepository _logDayRepository;

        public LogDayController(ILogDayRepository logDayRepository)
        {
            _logDayRepository = logDayRepository;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<LogDay> Get()
        {
            return _logDayRepository.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public LogDay Get(int id)
        {
            return _logDayRepository.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]LogDay logDay)
        {
            _logDayRepository.Add(logDay);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(LogDay logDay)
        {
            _logDayRepository.Update(logDay);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logDayRepository.Delete(id);
        }
    }
}
