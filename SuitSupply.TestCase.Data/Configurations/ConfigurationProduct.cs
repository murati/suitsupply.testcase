using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuitSupply.TestCase.Data.Models;

namespace SuitSupply.TestCase.Data.Configurations
{
    internal class ConfigurationProduct : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasMaxLength(25).IsRequired();
            builder.Property(p => p.Photo).HasMaxLength(255).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.LastUpdate).HasDefaultValueSql("getdate()").ValueGeneratedOnAddOrUpdate();
            builder.Property(p => p.RowVersion).IsRowVersion();
        }
    }
}
