using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Data.Configurations
{
    public class OrdersConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.HasKey(o => o.OrderId);

            builder.Property(o => o.TotalCost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.OrderDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(o => o.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.UpdatedByUser)
                .WithMany()
                .HasForeignKey(o => o.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.OrderStatusNavigation)
                .WithMany()
                .HasForeignKey(o => o.OrderStatus)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.PaymentTypeNavigation)
                .WithMany()
                .HasForeignKey(o => o.PaymentType)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.PaymentMethodNavigation)
                .WithMany()
                .HasForeignKey(o => o.PaymentMethod)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.PaymentStatusNavigation)
                .WithMany()
                .HasForeignKey(o => o.PaymentStatus)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
