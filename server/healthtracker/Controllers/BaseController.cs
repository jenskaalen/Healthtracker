using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using healthtracker.Model;
using healthtracker.Repository;
using Microsoft.AspNetCore.Mvc;

namespace healthtracker.Controllers
{
    public class BaseController<T>: Controller
    {
        private readonly IRepository<T> _repository;

        public BaseController(IRepository<T> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<T> Get()
        {
            return _repository.GetAll();
        }

        public T Get(int id)
        {
            return _repository.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public void Post(T entity)
        {
            _repository.Add(entity);
        }

        public void Put(T logtype)
        {
            _repository.Update(logtype);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
