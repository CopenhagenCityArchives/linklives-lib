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

To create and run migrations when using Visual Studio:
* Go to the Package-manager-console
* Set linkliveslib-dummy as startup project
* Chose linkliveslib
* Run ``add-migration A-MIGRATION-NAME-OF-YOUR-CHOOSING``
* To remove a migration run ``remove-migration``
* To update the database run ``update-database``

You can also follow this [guide on EF migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli).
