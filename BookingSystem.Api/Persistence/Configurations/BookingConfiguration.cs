using BookingSystem.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Api.Persistence.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.CustomerEmail)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.HasOne(b => b.Slot)
                .WithOne()
                .HasForeignKey<Booking>(b => b.SlotId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(b => b.SlotId)
                .IsUnique();

            builder.HasIndex(b => b.CustomerEmail);
        }
    }
}
