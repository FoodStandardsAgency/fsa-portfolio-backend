using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using FSAPortfolio.Application.Services.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Projects
{
    public class DataDumpService : BaseService, IDataDumpService
    {
        private readonly IProjectDataService projectDataService;
        private readonly IPortfolioConfigurationService portfolioConfigurationService;

        private readonly string DataDumpStorageConnectionString;
        private readonly string DataDumpStorageShareName;

        public DataDumpService(IServiceContext context, IProjectDataService projectDataService, IPortfolioConfigurationService portfolioConfigurationService) : base(context)
        {
            this.projectDataService = projectDataService;
            this.portfolioConfigurationService = portfolioConfigurationService;

            this.DataDumpStorageConnectionString = ConfigurationManager.AppSettings["DataDumpStorageConnectionString"];
            this.DataDumpStorageShareName = ConfigurationManager.AppSettings["DataDumpStorageShareName"];
        }

        public async Task DumpPortfolioConfig(string portfolio)
        {
            var config = await portfolioConfigurationService.GetConfigurationAsync(portfolio);
            await DumpJson(config, $"{portfolio}_config.json");
        }

        public async Task DumpPortfolioProjects(string portfolio, string[] ids = null)
        {
            var projects = await projectDataService.GetProjectDataAsync(portfolio, ids);
            await DumpJson(projects, $"{portfolio}_projects.json");
        }


        public async Task DumpProjectUpdates(string portfolio, string[] ids = null)
        {
            var updates = await projectDataService.GetProjectUpdateDataAsync(portfolio, ids);
            await DumpJson(updates, $"{portfolio}_updates.json");
        }

        public async Task DumpProjectChanges(string portfolio, string[] ids = null)
        {
            var changes = await projectDataService.GetProjectChangeDataAsync(portfolio, ids);
            await DumpJson(changes, $"{portfolio}_changes.json");
        }



        private async Task DumpJson<T>(T obj, string fileName)
        {
            // Dump to local temp file (need to do this because need to know file size before copying to Azure File Share).
            var tmpFile = Path.GetTempFileName();
            using (StreamWriter sw = new StreamWriter(tmpFile))
            using (JsonWriter jtw = new JsonTextWriter(sw))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(jtw, obj);
            }

            try
            {
                // Copy the temp file to Azure File Share
                ShareClient share = new ShareClient(DataDumpStorageConnectionString, DataDumpStorageShareName);
                ShareDirectoryClient directory = share.GetDirectoryClient("/");
                ShareFileClient file = directory.GetFileClient(fileName);

                using (FileStream stream = File.OpenRead(tmpFile))
                {
                    await file.CreateAsync(stream.Length);
                    await file.UploadRangeAsync(new HttpRange(0, stream.Length), stream);
                }

            }
            finally
            {
                // Clean up the temp file
                File.Delete(tmpFile);
            }
        }

    }
}
