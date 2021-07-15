using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using System.Globalization;
using CsvHelper.Configuration;

namespace FSAPortfolio.Utility.AzureDiagnostics
{
    public class AzureBlobClient
    {
        private DateTime today = DateTime.Today;
        private string environment;
        private string connectionString;
        private string appServiceName;
        private string appServiceDumpfile;
        private string webServerName;
        private string webServerDumpfile;
        private string containerName;
        private bool todayOnly;
        private bool silent;

        private BlobServiceClient blobServiceClient;

        private AzureBlobClient(bool todayOnly, string env)
        {
            this.todayOnly = todayOnly;
            this.environment = env;

            connectionString = ConfigurationManager.AppSettings[$"AzureStorageConnectionString.{env}"];
            appServiceName = ConfigurationManager.AppSettings[$"AppServiceName.{env}"];
            appServiceDumpfile = ConfigurationManager.AppSettings[$"AppServiceDumpFile.{env}"];
            webServerName = ConfigurationManager.AppSettings[$"WebServerName.{env}"];
            webServerDumpfile = ConfigurationManager.AppSettings[$"WebServerDumpFile.{env}"];
            silent = appServiceDumpfile == null;

            blobServiceClient = new BlobServiceClient(connectionString);

        }

        public static async Task OutputBlobText(bool todayOnly, bool web, string env)
        {
            try
            {
                var client = new AzureBlobClient(todayOnly, env);
                await client.Execute(web);
            }
            catch(IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task Execute(bool webLogs)
        {
            if(webLogs)
            {
                await ExecuteWebServerDownloadAsync();
            }
            else
            {
                await ExecuteAppServiceDownloadAsync();
            }
        }

        private async Task ExecuteWebServerDownloadAsync()
        {
            string containerName = ConfigurationManager.AppSettings["AzureWebServerStorageContainer"];
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            using (var writer = webServerDumpfile == null ? new StreamWriter(Console.OpenStandardOutput()) : new StreamWriter(webServerDumpfile))
            {
                if (!silent)
                {
                    Console.WriteLine($"Getting Azure web server logs for {environment} environment...");
                    Console.WriteLine($"Dumping logs to file: {webServerDumpfile}");
                }

                // App Service name is the prefix to filter on.
                var x = containerClient.GetBlobs().ToList();
                await foreach (var item in containerClient.GetBlobsAsync(prefix: webServerName))
                {
                    if (!silent)
                    {
                        Console.WriteLine($"Dumping {item.Name}");
                    }
                    var blobClient = containerClient.GetBlobClient(item.Name);
                    var download = await blobClient.DownloadContentAsync();

                    using (var reader = new StreamReader(blobClient.DownloadStreaming().Value.Content))
                    {
                        writer.Write(await reader.ReadToEndAsync());
                    }
                }

            }
        }

        private async Task ExecuteAppServiceDownloadAsync()
        {
            string containerName = ConfigurationManager.AppSettings["AzureAppServiceStorageContainer"];
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            using (var writer = appServiceDumpfile == null ? new StreamWriter(Console.OpenStandardOutput()) : new StreamWriter(appServiceDumpfile))
            {
                if (!silent)
                {
                    Console.WriteLine($"Getting Azure diagnostic logs for {environment} environment...");
                    Console.WriteLine($"Dumping logs to file: {appServiceDumpfile}");
                }

                // Set up date check function (used if todayOnly is true)
                // File names are of the form...
                // FSAPortfolioBackend/2021/06/28/08/2796f7-7260.applicationLog.csv
                var pattern = @$"{appServiceName}/{today.Year}/{today.Month:D2}/{today.Day:D2}/\d{{2}}/.*\.applicationLog\.csv";
                Func<BlobItem, bool> dateCheckFunc = (item) =>
                {
                    var match = Regex.Match(item.Name, pattern);
                    return match.Success;
                };

                // Read in all lines, then order by date and output
                List<entry> outputLines = new List<entry>();
                var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                var config = new CsvConfiguration(culture);

                // App Service name is the prefix to filter on.
                var x = containerClient.GetBlobs().ToList();
                await foreach (var item in containerClient.GetBlobsAsync(prefix: appServiceName))
                {
                    if (!todayOnly || dateCheckFunc(item))
                    {
                        var blobClient = containerClient.GetBlobClient(item.Name);
                        var download = await blobClient.DownloadContentAsync();

                        if (!silent)
                        {
                            Console.WriteLine($"Dumping {item.Name}");
                        }

                        using (var reader = new StreamReader(blobClient.DownloadStreaming().Value.Content))
                        using (var csv = new CsvReader(reader, config))
                        {
                            var records = csv.GetRecords<entry>().ToList();
                            outputLines.AddRange(records);
                        }
                    }
                }

                // Output dates
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(outputLines.OrderBy(l => l.date));
                }
            }
        }
    }

    public class entry
    {
        public DateTime date { get; set; }
        public string level { get; set; }
        public string applicationName { get; set; }
        public string instanceId { get; set; }
        public long eventTickCount { get; set; }
        public int eventId { get; set; }
        public int pid { get; set; }
        public int tid { get; set; }
        public string message { get; set; }
        public string activityId { get; set; }
    }
}
