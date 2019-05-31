using Healthtracker.Web.Model;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Healthtracker.Web.Repositories
{
    public class RavenDbRepository : ILogRepository
    {
        DocumentStore _store;

        public RavenDbRepository()
        {
            _store = GetDocumentStore();
            _store.Initialize();
        }

        public Log Create(Log log)
        {
            return CreateOrUpdate(log);
        }

        private Log CreateOrUpdate(Log log)
        {
            using (var session = _store.OpenSession())
            {
                log.DocumentId = $"logs/{log.Id}";

                var existingLog = session.Load<Log>(log.DocumentId);

                if (existingLog != null)
                {
                    existingLog.Activity = log.Activity;
                    existingLog.Comment = log.Comment;
                    existingLog.Date = log.Date;
                    existingLog.Feeling = log.Feeling;
                    session.SaveChanges();
                    return existingLog;
                }

                //does not exist - meaning we create
                session.Store(log);
                session.SaveChanges();
            }

            return log;
        }

        public void Delete(int id)
        {
            using (var session = _store.OpenSession())
            {
                var log = new Log() { Id = id };

                session.Delete(log);
                session.SaveChanges();
            }
        }

        public List<Log> GetAll(string userId)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Log>()
                    .Where(x => x.UserId == userId)
                    .ToList();
            }
        }

        public Log Update(Log log)
        {
            var result = CreateOrUpdate(log);
            return result;
        }

        private DocumentStore GetDocumentStore()
        {
            return new DocumentStore()
            {
                Urls = new string[] { "http://10.0.0.95:8080" },
                Database = "LogDb",
                Conventions =
                    {
                        FindIdentityProperty = prop => prop.Name == "DocumentId"
                    }
            };
        }
    }
}
