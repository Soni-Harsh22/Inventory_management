using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Data.Configurations;

public class StockPurchaseOrdersConfiguration : IEntityTypeConfiguration<StockPurchaseOrders>
{
    public void Configure(EntityTypeBuilder<StockPurchaseOrders> builder)
    {
        builder.HasKey(po => po.StockPurchaseOrderId);

        

        //builder.HasOne(po => po.OrderByUser)
        //       .WithMany()
        //       .HasForeignKey(po => po.PurchaseBy)
        //       .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.UpdatedByUser)
               .WithMany()
               .HasForeignKey(po => po.UpdatedBy)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.VendorDetails)
               .WithMany()
               .HasForeignKey(po => po.VendorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.PaymentStatus)
               .WithMany()
               .HasForeignKey(po => po.PaymentStatusId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.PaymentMethod)
               .WithMany()
               .HasForeignKey(po => po.PaymentMethodId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.PaymentType)
               .WithMany()
               .HasForeignKey(po => po.PaymentTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.OrderStatus)
               .WithMany()
               .HasForeignKey(po => po.OrderStatusId)
               .OnDelete(DeleteBehavior.Restrict);


        builder.HasMany(x => x.StockPurchaseOrderItems)
               .WithOne(x => x.StockPurchaseOrder)
               .HasForeignKey(x => x.StockPurchaseOrderId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
