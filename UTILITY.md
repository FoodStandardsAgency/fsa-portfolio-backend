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

Diagnostic log examples:
```
portfolio azlogs --today
portfolio azlogs --days 14
portfolio azlogs --env TEST
```

Web log examples:
```
portfolio azlogs --web --today
portfolio azlogs --web --days 14
portfolio azlogs --web --env TEST
```


## `markdown` - Output documentation for API as markdown
Running this processes the inline documentation tags and generates a markdown document for the project fields in the API. Default descriptions are used where fields have no inline documentation.

Examples:
```
portfolio markdown --file
```

## Configuration
This command requires an `appSettings.config` file in the `FSAPortfolio.Utility` project directory with the following keys set:

```
<appSettings>
	<!-- LOGS LIVE -->
	<add key="AzureStorageConnectionString.LIVE" value="...Obtain this from the Storage configuration in the Azure Portal..."/>
	<add key="AppServiceName.LIVE" value="...Obtain this value from Azure Portal..."/>
	<add key="WebServerName.LIVE" value="...Obtain this value from Azure Portal..."/>
	<add key="AppServiceDumpFile.LIVE" value="c:\tmp\portfoliolive.csv"/>
	<add key="WebServerDumpFile.LIVE" value="c:\tmp\portfoliolive.log"/>


	<!-- LOGS TEST -->
	<add key="AzureStorageConnectionString.TEST" value="...Obtain this from the Storage configuration in the Azure Portal..."/>
	<add key="AppServiceName.TEST" value="...Obtain this value from Azure Portal..."/>
	<add key="WebServerName.TEST" value="...Obtain this value from Azure Portal..."/>
	<add key="AppServiceDumpFile.TEST" value="c:\tmp\portfoliotest.csv"/>
	<add key="WebServerDumpFile.TEST" value="c:\tmp\portfoliotest.log"/>

	<add key="AzureAppServiceStorageContainer" value="appservice"/>
	<add key="AzureWebServerStorageContainer" value="webserver"/>

	<!-- Backend API -->
	<add key="portfolio" value="<... portfolio abbreviation ...>"/>
	<add key="testAdminUser" value="<... user name ...>"/>
	<add key="testAdminUserPassword" value="<... password ...>"/>
	<add key="backendUrl" value="http://localhost/FSAPortfolio.WebAPI/"/>
	<add key="backendApiKey" value="<... the key you configured ...>"/>

	<add key="FieldsMdOutputPath" value="<... path to ...>\repos\fsa-portfolio\docs\FIELDS.md"/>

</appSettings>

```


