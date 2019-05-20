using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Healthtracker.Web.Data;
using Healthtracker.Web.Model;
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

        public LogController(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Log
        [HttpGet]
        public IEnumerable<Log> Get()
        {
            //TODO: get root path
            using (var db = new LiteDatabase(_hostingEnvironment.WebRootPath + @"\..\Data\logdata.db"))
            {
                string userId = User.Identity.Name;
                var col = db.GetCollection<Log>("logs");
                return col.Find(x => x.UserId == userId).ToList();
                //return col.FindAll();
            }
        }

        // GET: api/Log/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Log
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Log/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
