using BookingSystem.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Api.Persistence.Configurations
{
    public class SlotConfiguration : IEntityTypeConfiguration<Slot>
    {
        public void Configure(EntityTypeBuilder<Slot> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StartTime)
            .IsRequired();

            builder.Property(s => s.EndTime)
                .IsRequired();

            builder.Property(s => s.IsBooked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasIndex(s => new { s.IsBooked, s.StartTime });
        }
    }
}
