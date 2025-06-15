using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasOne(c => c.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(c => c.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(c => c.UpdatedByUser)
            //       .WithMany()
            //       .HasForeignKey(c => c.UpdatedBy)
            //       .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
