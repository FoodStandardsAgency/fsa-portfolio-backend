using FSAPortfolio.Utility.AzureDiagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Invocation;
using FSAPortfolio.Utility.Docs;

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

            var fieldDocsCommand = new Command("markdown", "Generate markdown files")
            {
                new Option<string>("--file", getDefaultValue: () => "fields", "Generate markdown file for project fields.")
            };

            var rootCommand = new RootCommand("Portfolio utitlies")
            {
                logCommand, fieldDocsCommand
            };

            logCommand.Handler = CommandHandler.Create<bool, string>(async (today, env) => await AzureBlobClient.OutputBlobText(today, env));
            fieldDocsCommand.Handler = CommandHandler.Create<string>(async (file) => await MarkdownGenerator.OutputFile(file));

            return await rootCommand.InvokeAsync(args);

        }
    }
}
