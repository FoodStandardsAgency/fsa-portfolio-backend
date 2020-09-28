# fsa-portfolio-backend

## Database Migrations
When using database migrations:
* Update-Database by convention looks at connection strings in the **startup project**, unless you manually specify the project as follows:
	* `Update-Database -ProjectName FSAPortfolio.Entities` 
* Generate a release script as follows:
	* `Update-Database -Script -SourceMigration: $InitialDatabase -TargetMigration: InitialCreate`

