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
using FSAPortfolio.Application.Models;
using System.Net;
using FSAPortfolio.Application.Services;
using FSAPortfolio.Application.Services.Sync;
using System.Threading.Tasks;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class SyncController : ApiController
    {
        private readonly ISyncService syncService;

        public SyncController(ISyncService syncService)
        {
            this.syncService = syncService;
        }

        // GET: api/Sync/SyncAll
        [AcceptVerbs("GET")]
        public async Task<IEnumerable<string>> SyncAll([FromUri] bool syncPortfolios = true)
        {
            if (syncPortfolios)
            {
                syncService.ClearLog();
                syncService.SyncUsers();
                syncService.SyncDirectorates();
                syncService.SyncPortfolios();
                await syncService.SyncPeople();
            }

            syncService.SyncAllProjects();
            return syncService.Messages();
        }

        // GET: api/Sync/SyncUsers
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncUsers()
        {
            syncService.ClearLog();
            syncService.SyncUsers();
            return syncService.Messages();
        }

        // GET: api/Sync/SyncPeople
        [AcceptVerbs("GET")]
        public async Task<IEnumerable<string>> SyncPeople()
        {
            syncService.ClearLog();
            await syncService.SyncPeople(forceADSync: true);
            return syncService.Messages();
        }

        // GET: api/Sync/SyncPortfolios
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncPortfolios()
        {
            syncService.ClearLog();
            syncService.SyncPortfolios();
            return syncService.Messages();
        }



        // PUT: api/Sync/SyncProject
        [AcceptVerbs("PUT")]
        public IEnumerable<string> SyncProject([FromBody] SyncRequestModel syncRequest)
        {
            List<string> messages;
            try
            {
                syncService.ClearLog();
                if (!syncService.SyncProject(syncRequest.ProjectId))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                messages = syncService.Messages().ToList();
            }
            catch(Exception e)
            {
                messages = syncService.Messages().ToList();
                messages.Add(e.Message);
                messages.Add(e.StackTrace);
            }
            return messages;
        }

        // GET: api/Sync/SyncAllProjects
        [AcceptVerbs("GET")]
        public IEnumerable<string> SyncAllProjects([FromUri] string portfolio = null)
        {
            syncService.ClearLog();
            syncService.SyncAllProjects(portfolio);
            return syncService.Messages();
        }
    }
}
