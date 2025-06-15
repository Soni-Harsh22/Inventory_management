using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Data.Configurations
{
    public class StocksMovementConfiguration : IEntityTypeConfiguration<StocksMovement>
    {
        public void Configure(EntityTypeBuilder<StocksMovement> builder)
        {
            builder.HasKey(sm => sm.MovementId);

            builder.HasOne(sm => sm.Product)
                   .WithMany()
                   .HasForeignKey(sm => sm.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sm => sm.MovementType)
            .WithMany()
                   .HasForeignKey(sm => sm.MovementTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne<Users>()
            //       .WithMany()
            //       .HasForeignKey(sm => sm.UpdateBy)
            //       .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sm => sm.UpdatedByUser)
       .WithMany()
       .HasForeignKey(sm => sm.UpdateBy)
       .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
