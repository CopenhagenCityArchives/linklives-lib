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
            var dbConn = @"server=***REMOVED***;***REMOVED***;pwd=***REMOVED***;database=***REMOVED***";
            var dbContext = new LinklivesContext(new DbContextOptionsBuilder<LinklivesContext>().UseMySQL(dbConn).Options);
            return dbContext;
        }
    }
}
