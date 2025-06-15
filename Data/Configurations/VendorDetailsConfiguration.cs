using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Data.Configurations
{
    public class VendorDetailsConfiguration : IEntityTypeConfiguration<VendorDetails>
    {
        public void Configure(EntityTypeBuilder<VendorDetails> builder)
        {
            // Foreign key: CreatedBy
            builder.HasOne(v => v.CreatedByUser)
                .WithMany()
                .HasForeignKey(v => v.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Foreign key: UpdatedBy
            //builder.HasOne(v => v.UpdatedByUser)
            //    .WithMany()
            //    .HasForeignKey(v => v.UpdatedBy)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
