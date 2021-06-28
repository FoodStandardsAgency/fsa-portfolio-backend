# FSAPortfolio.Utility Console Application
The FSAPortfolio.Utility project is a utility application for developers to simplify maintenance tasks. It is run from the command line.
The `portfolio.exe` executable is in the build output directory (e.g. `./FSAPortfolio.Utility/bin/Debug/netcoreapp3.1/portfolio.exe`).

The general usage is:
```
portfolio [command] [command options]
```

The following commands are available...

## `azlogs` - Output Azure BLOB Storage Logs
Running this console app concatenates the BLOB Storage application logs for the App Service and outputs the text to the console.
Each log is in CSV format and the first line is the column headers: this is only written out for the first log so the output can be dumped as a single CSV file.
The `--today` option can be used to limit the output to today's logs only.

This command requires an `appSettings.config` file in the `FSAPortfolio.Utility` project directory with the following keys set:

```
<appSettings>
	<add key="AzureStorageConnectionString" value="...Obtain this from the Storage configuration in the Azure Portal..."/>
	<add key="AzureStorageContainer" value="appservice"/>
	<add key="AppServiceName" value="FSAPortfolioBackend"/>
</appSettings>
```

Examples:
```
portfolio azlogs > /c/tmp/portfoliolive.csv
portfolio azlogs --today > /c/tmp/portfoliolive.csv
```

