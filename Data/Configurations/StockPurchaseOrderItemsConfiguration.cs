using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Data.Configurations
{
    public class StockPurchaseOrderItemsConfiguration : IEntityTypeConfiguration<StockPurchaseOrderItems>
    {
        public void Configure(EntityTypeBuilder<StockPurchaseOrderItems> builder)
        {
            builder.HasKey(s => s.StockPurchaseOrderItemId);

            builder.Property(s => s.ProductPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.TotalPrice)
                .HasColumnType("decimal(18,2)");


            builder.HasOne(s => s.StockPurchaseOrder)
                .WithMany(p => p.StockPurchaseOrderItems)
                .HasForeignKey(s => s.StockPurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(s => s.Product)
                .WithMany(p => p.StockPurchaseOrderItems)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.OrderStatus)
                .WithMany(p => p.StockPurchaseOrderItems)
                .HasForeignKey(s => s.OrderItemStatus)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
