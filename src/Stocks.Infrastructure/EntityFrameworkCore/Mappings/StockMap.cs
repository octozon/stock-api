using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stocks.Domain.StockItems;
using Stocks.Domain.Stocks;

namespace Stocks.Infrastructure.EntityFrameworkCore.Mappings
{
    internal class StockMap : IEntityTypeConfiguration<Stock>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stocks");

            builder.HasKey(p => p.Id);
            
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

            builder.Property(p => p.Name)
                .HasColumnName("Name")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(p => p.Name)
                .IsUnique();

            builder.HasMany<StockItem>()
                .WithOne()
                .HasForeignKey(p => p.StockId);
        }
    }
}