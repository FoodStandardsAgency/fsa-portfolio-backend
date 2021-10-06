using Elasticsearch.Net;
using FSAPortfolio.Application.Models;
using FSAPortfolio.Application.Services.Index.Nest;
using FSAPortfolio.Application.Services.Projects;
using FSAPortfolio.Entities.Projects;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace FSAPortfolio.Application.Services.Index
{
    public class SearchService : BaseService, ISearchService
    {
        private IProjectDataService projectDataService;

        public SearchService(IServiceContext context, IProjectDataService projectDataService) : base(context)
        {
            this.projectDataService = projectDataService;
        }

        public async Task SearchProjectIndexAsync()
        {
            var nestClient = new ProjectNestClient();
            await nestClient.SearchProjectIndex();
        }
    }

}
