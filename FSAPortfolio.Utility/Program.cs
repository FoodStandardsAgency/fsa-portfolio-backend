using FSAPortfolio.Utility.AzureDiagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace FSAPortfolio.Utility
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var logCommand = new Command("azlogs", "Download portfolio backend diagnostic log files from an Azure Blob Storage Container")
            {
                new Option<bool>("--today", getDefaultValue: () => false, "Only output logs generated today"),
                new Option<string>("--env", getDefaultValue: () => "LIVE", "The environment to download from. Requires settings for each environment to be configured in appSettings e.g. AzureStorageConnectionString.LIVE & AzureStorageConnectionString.TEST.")
            };
            var rootCommand = new RootCommand("Portfolio utitlies")
            {
                logCommand
            };

            logCommand.Handler = CommandHandler.Create<bool, string>(async (today, env) => await AzureBlobClient.OutputBlobText(today, env));

            return await rootCommand.InvokeAsync(args);

        }
    }
}
