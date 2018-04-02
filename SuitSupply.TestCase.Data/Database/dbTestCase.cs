using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SuitSupply.TestCase.Data.Configurations;
using SuitSupply.TestCase.Data.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace SuitSupply.TestCase.Data.Database
{
    public class dbTestCase : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public dbTestCase(DbContextOptions options) : base(options)
        {
 
        }

        public dbTestCase() : base()
        {
        }
 
    
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ConfigurationProduct());
        }
    }
}
