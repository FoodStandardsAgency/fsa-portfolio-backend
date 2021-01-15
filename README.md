# fsa-portfolio-backend

## Requirements:
### Portfolio instance
* IIS
* SQL Server
	* Create a blank database and give permissions to the IIS application pool running the web api.

### Recommended for development bootstrapping
* GitBash
* Chocolatey Nuget
* jq
	* chocolatey install jq

### Optional useful tools
* Python
* Grip for viewing README.md (see https://github.com/joeyespo/grip)
	* pip install grip

## Setting up a Local Portfolio Instance
You may want to do this for development purposes, or simply to run a portfolio to manage your own tasks.
Here is a breakdown of the steps to set up a portfolio instance:
* Create and [initialise](#database-initialisation) an empty database
* Create a secrets script, ```setSecrets.sh```, to hold sensitive data (this is ignored by git)
* Run the secrets and bootstrapping scripts

These steps are described in detail in the following sections.

### Database initialisation
This creates the database tables and constraints. 
It can either be done using EF6 migrations in the package manager console, 
or by running the deployment scripts generated for each migration 
(this is how we deploy database updates to the cloud where running migrations is not feasible).

#### In Visual Studio using the Package Manager Console

#### From the Deployment Scripts

### Bootstrapping the instance
This is how to bootstrap a fresh development instance.

1. [Initialise a database as instructed above](#database-initialisation).
2. In a gitbash window, cd to the Bootstrapping scripts directory 
3. Create a ```setSecrets.sh``` script file to set the following variables...
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
4. Run this script in the current window using the ```source``` command:
	* ```source setSecrets.sh```
5. Run the bootstrapping script, again in the current window:
	* ```source bootstrapPortfolio.sh```
	* ...the bootstrapping process then:
		* initialises access groups
		* creates a user
		* creates a portfolio
		* gives the user permissions on the portfolio



## Database Migrations
When using database migrations:
* Update-Database by convention looks at connection strings in the **startup project**, unless you manually specify the project as follows:
	* `Update-Database -ProjectName FSAPortfolio.Entities` 
* Generate a release script as follows:
	* `Update-Database -Script -SourceMigration: $InitialDatabase -TargetMigration: InitialCreate`
