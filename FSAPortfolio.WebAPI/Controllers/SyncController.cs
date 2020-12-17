using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
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
using System.Threading.Tasks;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class SyncController : ApiController
    {
        // GET: api/Sync/SyncAll
        [AcceptVerbs("GET")]
        public async Task<IEnumerable<string>> SyncAll([FromUri] bool syncPortfolios = true)
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages);

            if (syncPortfolios)
            {
                sync.SyncUsers();
                sync.SyncDirectorates();
                sync.SyncPortfolios();
                await sync.SyncPeople();
            }

            sync.SyncAllProjects();
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
        public async Task<IEnumerable<string>> SyncPeople()
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages);
            await sync.SyncPeople(forceADSync: true);
            return messages;
        }

        // GET: api/Sync/SyncPortfolios
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncPortfolios()
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages);
            sync.SyncPortfolios();
            return messages;
        }



        // PUT: api/Sync/SyncProject
        [AcceptVerbs("PUT")]
        public IEnumerable<string> SyncProject([FromBody] SyncRequestModel syncRequest)
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages, true);

            try
            {
                if (!sync.SyncProject(syncRequest.ProjectId))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
            catch(Exception e)
            {
                messages.Add(e.Message);
                messages.Add(e.StackTrace);
            }
            return messages;
        }

        // GET: api/Sync/SyncAllProjects
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncAllProjects([FromUri] string portfolio = null)
        {
            List<string> messages = new List<string>();
            var sync = new SyncProvider(messages);
            sync.SyncAllProjects(portfolio);
            return messages;
        }
    }
}
