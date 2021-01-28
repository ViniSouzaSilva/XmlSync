using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XmlSync.EntityFramework.Models;

namespace XmlSync.EntityFramework
{
   public  class XmlSyncDbContext : DbContext
    {
        public XmlSyncDbContext(DbContextOptions<XmlSyncDbContext> options) :base(options)
        {

        }
        public DbSet<XML_INFO> XML_INFOs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
