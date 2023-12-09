using VezeetaProject.Core.Models;

namespace VezeetaProject.Core.EntitysConfiguration
{
    public class AppointmentTimesEntityConfiguration : IEntityTypeConfiguration<AppointmentTimes>
    {
        public void Configure(EntityTypeBuilder<AppointmentTimes> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Time)
                 .IsRequired();

            builder.Property( p => p.DoctorAppointmentId)
            .IsRequired();

            builder.HasMany(p => p.Booking)
                   .WithOne(p => p.AppointmentTimes)
                   .HasForeignKey(p => p.AppointmentTimesId);
        }
    }
}
