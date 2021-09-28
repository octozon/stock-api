using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stocks.Domain.StockItems;
using Stocks.Domain.Stocks;

namespace Stocks.Infrastructure.EntityFrameworkCore.Mappings
{
    internal class StockItemMap : IEntityTypeConfiguration<StockItem>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<StockItem> builder)
        {
            builder.ToTable("StockItems");

            builder.HasKey(p => p.Id);
            
            builder.Property<uint>("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

            builder.Property(p => p.ProductId)
                .HasColumnName("Product_Id")
                .IsRequired();

            builder.OwnsOne(p => p.Position, position =>
            {
                position.Property(p => p.Number)
                    .HasColumnName("Position_Number")
                    .IsRequired();
            });
            builder.Navigation(p => p.Position).IsRequired();

            builder.OwnsOne(p => p.State, state =>
            {
                state.Property(p => p.CurrentState)
                    .HasColumnName("State_CurrentState")
                    .HasMaxLength(255)
                    .IsRequired();
            });
            builder.Navigation(p => p.State).IsRequired();
        }
    }
}