using VezeetaProject.Core.Consts;

namespace VezeetaProject.Core.EntitysConfiguration
{
    public class BookingEntityConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PatientId)
                   .IsRequired();

            builder.Property(p => p.DoctorId)
                   .IsRequired();

            builder.Property(p => p.DiscountId)
                  .IsRequired(false);

            builder.Property(p => p.RequestStauts).
                    HasDefaultValue(RequestStatus.Pending);
        }
    }
}
