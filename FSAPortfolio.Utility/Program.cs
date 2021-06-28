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
            var logCommand = new Command("azlogs", "Download log files from Azure Blob Storage Container")
            {
                new Option<bool>("--today", getDefaultValue: () => false, "Only output logs generated today")
            };
            var rootCommand = new RootCommand("Portfolio utitlies")
            {
                logCommand
            };

            logCommand.Handler = CommandHandler.Create<bool>(async (today) => await BlobContainerClient.OutputBlobText(today));

            return await rootCommand.InvokeAsync(args);

        }
    }
}
