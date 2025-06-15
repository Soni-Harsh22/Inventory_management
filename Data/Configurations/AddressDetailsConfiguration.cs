using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Data.Configurations
{
    public class AddressDetailsConfiguration : IEntityTypeConfiguration<AddressDetails>
    {
        public void Configure(EntityTypeBuilder<AddressDetails> builder)
        {
            builder.HasKey(a => a.AddressId);

            builder.HasOne<Users>()
                   .WithMany()
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
