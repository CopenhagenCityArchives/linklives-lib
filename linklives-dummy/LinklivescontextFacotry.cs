using Linklives.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace linklives_dummy
{
    class LinklivesContextFactory : IDesignTimeDbContextFactory<LinklivesContext>
    {
        public LinklivesContext CreateDbContext(string[] args)
        {
            //TODO: The new dataset changes didnt migrate cleanly to the test db so it was nescesary to drop all tables and wipe the migration history to do a new create from scratch. This will also need to be done on prod.
            //TODO: Before wiping prod database remember to take backups of the RatingOptions table so that the data can be rolled back on afterwards.
            //This factory is used by EF when running migrations to db, change dbConn to connect to different databases
            //var dbConn = @"server=***REMOVED***;***REMOVED***;pwd=***REMOVED***;database=***REMOVED***"; //Test db
            var dbConn = @"server=127.0.0.1;uid=root;pwd=123456;database=***REMOVED***";
            var dbContext = new LinklivesContext(new DbContextOptionsBuilder<LinklivesContext>().UseMySQL(dbConn).Options);

            return dbContext;
        }
    }
}
