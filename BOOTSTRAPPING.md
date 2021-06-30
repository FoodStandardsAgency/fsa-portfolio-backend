# Bootstrapping a Development Instance

## Requirements:
### Portfolio instance
* IIS
* SQL Server
	* Create a blank database and give permissions to the IIS application pool running the web api.

### Recommended for development bootstrapping
* GitBash

### Optional useful tools
* Visual Studio 2019 (or latest)
* SQL Server Management Studio (SSMS)
* Python
* Grip for viewing README.md (see https://github.com/joeyespo/grip)

	> $ pip install grip

* Chocolatey Nuget
	* Used to install `jq`
	* The open source version will suffice
	* Note that you must set the execution policy (see chocolatey docs) to allow package installation
* jq - a tool for processing JSON at the command line
	> Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
	> $ chocolatey install jq



---
## Setting up a Local Portfolio Instance
You may want to do this for development purposes, or simply to run a portfolio to manage your own tasks.
Here is a breakdown of the steps to set up a portfolio instance:
* [Database initialisation](#database-initialisation) Create and [initialise](#database-initialisation) an empty database
* [Bootstrapping the instance][bootstrap] Create a secrets script, `setSecrets.sh`, to hold sensitive data (this is ignored by git)
	* Run the secrets and bootstrapping scripts

These steps are described in detail in the following sections.


### Database initialisation
This creates the database tables and constraints. 
It can be done either:
* using EF6 migrations in the package manager console, 
or;
* by running the deployment scripts generated for each migration 
(this is how we deploy database updates to the cloud where running migrations is not feasible).


#### In Visual Studio using the Package Manager Console
1. Create a `connectionStrings.config` file in FSAPortfolio.Entities project
	* Note that this file is include in the VS solution, but ignored by git. It's up to you to create the actual file after you clone the repository.
	* So if you named your database "DevPortfolio", the contents should be something like: 
	```
	<connectionStrings>
		<add name="PortfolioContext" connectionString="data source=localhost;initial catalog=DevPortfolio;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />
	</connectionStrings>

	```
2. :warning: Make sure the `FSA.Portfolio.Entities` project is set as the *Startup Project* for the solution (see [Database Migrations](#database-migrations) for explanation of why)
3. Open the Package Manager Console (View -> Other Windows -> Package Manager Console)
4. Type: `PM> Update-Database`
	* This will run the individual migrations on your database to bring it up-to-date.


#### From the Deployment Scripts
The deployment scripts can be found in the directory `...\fsa-portfolio-backend\FSAPortfolio.Database\Deployment`
and have a two digit prefix giving the order of execution. 
The scripts are SQL Server T-SQL and can be executed in SSMS queries.

> :warning: Make sure you select the correct database and don't run these on the `master` database. I do this from time to time and is a pain in the neck to unwind!

At the time of writing, there are 4 deployment scripts - the initial create and 3 migration scripts:

```
00_Create.sql
01_GenericSettings.sql
02_PortfolioRequiredRoles.sql
03_Archive.sql
```

Run these in order (in SSMS) to create an up-to-date database schema.

> :warning: There is also a script `00_DropAllTables.sql` that removes all tables from the database. 
You don't need this during bootstrapping - only if you want to start again with the database initialisation.
This file isn't always up to date; if not, it is manually generated in SSMS script generation wizard. Create your own by selecting the `Script DROP` option in *Advanced* settings. 


### Bootstrapping the instance
[bootstrap]: #bootstrapping-the-instance "Bootstrapping the instance"
This is how to bootstrap a fresh development instance.

1. [Initialise a database as instructed above](#database-initialisation).
2. In a gitbash window, cd to the Bootstrapping scripts directory 
3. Create a `setSecrets.sh` script file to set the following variables...
	```
	apiUrl=http://localhost/FSAPortfolio.WebAPI/api
	apiKey=<your api key here>
	apiUser=<user name>
	apiUserPassword=<user password>
	apiUserPasswordHash=<SHA256 hash of user password>
	apiUserAccessGroup=<admin|editor|superuser>
	apiPortfolioKey=<e.g. work>
	apiUserPortfolioRole=<Admin|Editor|Superuser>
	```
4. Run this script in the current window using the `source` command:
	> `$ source setSecrets.sh`
5. Run the bootstrapping script, again in the current window:
	> `$ source bootstrapPortfolio.sh`
	- ...the bootstrapping process then:
		- initialises access groups
		- creates a user
		- creates a portfolio
		- gives the user permissions on the portfolio



## Database Migrations
When using database migrations:
* Update-Database by convention looks at connection strings in the **startup project**, unless you manually specify the project as follows:
	> `PM> Update-Database -ProjectName FSAPortfolio.Entities` 
* Generate a release script as follows:
	> `PM> Update-Database -Script -SourceMigration: $InitialDatabase -TargetMigration: InitialCreate`



