using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace XmlSync.EntityFramework
{
    public class XmlSyncFactory : IDesignTimeDbContextFactory<XmlSyncDbContext>
    {
        public XmlSyncDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<XmlSyncDbContext>();
            options.EnableSensitiveDataLogging();
            options.UseSqlite("datasource=XmlSync.db");

            return new XmlSyncDbContext(options.Options);
        }
    }
}
