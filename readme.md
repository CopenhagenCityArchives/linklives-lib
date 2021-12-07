# linklives-lib
A common library for manipulation, loading and indexation of Link Lives data.

# Dependent projects
These projects use this library:
* Linklives-api
* Linklives-indexer-new

# Development
This project contains data and infrastructure for local development of the dependent projects.

Use ``docker-compose -f docker-compose-dev.yml`` to start a local mysql server and elasticsearch server.

When running it for the first time, follow the database migration guide below to setup the local database.

# Database migrations
When changes are made to the entities in this library, it may have impact on the expected database structure.
When database-changing changes are run, use the commands below to create a new migration.

* Note that the migrations are made on the database described in the linklives-dummy project
* Install the ef migration tool: ``dotnet tool install --global dotnet-ef``
* Add a migration: ``dotnet ef migrations add "MIGRATION_NAME" --project linklives-lib --startup-project linklives-lib-database-migrations``
* Update the database with the migration: ``dotnet ef database update --project linklives-dummy --startup-project linklives-dummy``

You can also follow this [guide on EF migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli).
