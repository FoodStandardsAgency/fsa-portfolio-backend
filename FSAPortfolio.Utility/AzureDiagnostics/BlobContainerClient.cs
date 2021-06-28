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
    public class BlobContainerClient
    {
        private static DateTime today = DateTime.Today;
        public static async Task OutputBlobText(bool todayOnly = true)
        {
            var connectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
            var containerName = ConfigurationManager.AppSettings["AzureStorageContainer"];
            var appServiceName = ConfigurationManager.AppSettings["AppServiceName"];

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

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
            await foreach (var item in containerClient.GetBlobsAsync(prefix: appServiceName))
            {
                if (!todayOnly || dateCheckFunc(item))
                {
                    var blobClient = containerClient.GetBlobClient(item.Name);
                    var download = await blobClient.DownloadContentAsync();

                    using (var reader = new StreamReader(blobClient.DownloadStreaming().Value.Content))
                    using (var csv = new CsvReader(reader, config))
                    {
                        var records = csv.GetRecords<entry>().ToList();
                        outputLines.AddRange(records);
                    }
                }
            }

            // Output dates
            using (var writer = new StreamWriter(Console.OpenStandardOutput()))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(outputLines.OrderBy(l => l.date));
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
