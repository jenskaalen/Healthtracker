﻿using Healthtracker.Web.Model;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Healthtracker.Web.Repositories
{
    public class RavenLogRepository : ILogRepository
    {
        DocumentStore _store;

        public RavenLogRepository()
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
                //if (log.Id > 0)
                //    SetDocumentIdFromId(log);

                //var existingLog = log.DocumentId != null ? session.Load<Log>(log.DocumentId) : null;

                Log existingLog = log.Id > 0 ? session.Query<Log>().Where(x => x.Id == log.Id).FirstOrDefault() : null;

                if (existingLog != null)
                {
                    existingLog.Activity = log.Activity;
                    existingLog.Comment = log.Comment;
                    existingLog.Date = log.Date;
                    existingLog.Feeling = log.Feeling;
                    existingLog.RestingHeartrate = log.RestingHeartrate;
                    existingLog.Sleep = log.Sleep;
                    existingLog.FitbitActivities = log.FitbitActivities;
                    existingLog.Activities = log.Activities;
                    existingLog.Supplements = log.Supplements;
                    session.SaveChanges();
                    return existingLog;
                }

                //does not exist - meaning we create
                session.Store(log);
                session.SaveChanges();
            }

            return log;
        }

        private static void SetDocumentIdFromId(Log log)
        {
            log.DocumentId = $"logs/{log.Id}-A";
        }

        public void Delete(int id)
        {
            using (var session = _store.OpenSession())
            {
                Log existingLog = session.Query<Log>().Where(x => x.Id == id).FirstOrDefault();

                session.Delete(existingLog);
                session.SaveChanges();
            }
        }

        public List<Log> GetAll(string userId)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Log>()
                    .Where(x => x.UserId == userId)
                    .Include(x => x.FitbitActivities)
                    .ToList();
            }
        }

        public List<Log> Get(int logsPerPage, int logsToSkip, string userId)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Log>()
                    .Where(x => x.UserId == userId)
                    .Include(x => x.FitbitActivities)
                    .OrderByDescending(log => log.Date)
                    .Skip(logsToSkip)
                    .Take(logsPerPage)
                    .ToList();
            }
        }

        public Log Update(Log log)
        {
            var result = CreateOrUpdate(log);
            return result;
        }

        public List<Log> Search(string text, string userId)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Log>()
                    .Where(x => x.UserId == userId)
                    .Search(x => x.Comment, $"*{text}*")
                    //.Include(x => x.FitbitActivities)
                    .OrderByDescending(log => log.Date)
                    .ToList();
            }
        }

        private DocumentStore GetDocumentStore()
        {
            X509Certificate2 clientCertificate = new X509Certificate2("certi.pfx", "FEANturi2");

            return new DocumentStore()
            {
                Urls = new string[] { "https://a.healthbonto.ravendb.community:4343" },
                Certificate = clientCertificate,
                Database = "LogDb",
                Conventions =
                    {
                        FindIdentityProperty = prop => prop.Name == "DocumentId"
                    }
            };
        }
    }
}