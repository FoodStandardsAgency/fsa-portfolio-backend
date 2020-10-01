﻿using FSAPortfolio.Entites;
using FSAPortfolio.Entites.Projects;
using FSAPortfolio.Entites.Users;
using FSAPortfolio.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Data.Entity;
using FSAPortfolio.WebAPI.Models;
using System.Net;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Sync;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class SyncController : ApiController
    {
        // GET: api/Sync/SyncAll
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncAll()
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages);
            sync.SyncUsers();
            sync.SyncStatuses();
            sync.SyncPeople();
            return messages;
        }

        // GET: api/Sync/SyncUsers
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncUsers()
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages);
            sync.SyncUsers();
            return messages;
        }

        // GET: api/Sync/SyncPeople
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncPeople()
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages);
            sync.SyncPeople();
            return messages;
        }

        // GET: api/Sync/SyncStatuses
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncStatuses()
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages);
            sync.SyncStatuses();
            return messages;
        }

        // PUT: api/Sync/SyncProject
        [AcceptVerbs("PUT")]
        public IEnumerable<string> SyncProject([FromBody] SyncRequestModel syncRequest)
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages);
            if (!sync.SyncProject(syncRequest.ProjectId))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return messages;
        }

        // GET: api/Sync/SyncAllProjects
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncAllProjects()
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages);
            sync.SyncAllProjects();
            return messages;
        }
    }
}
