namespace VezeetaProject.Core.EntitysConfiguration
{
    public class DoctorAppointmentEntityConfiguration : IEntityTypeConfiguration<DoctorAppointment>
    {
        public void Configure(EntityTypeBuilder<DoctorAppointment> builder)
        {
            builder.HasKey(p => p.Id);

         
            builder.Property(p => p.DoctorId).
                    IsRequired();

            builder.Property(p => p.Day)
                   .IsRequired();

            builder.HasMany(p => p.appointmentTimes)
                   .WithOne(p => p.DoctorAppointment)
                   .HasForeignKey(p => p.DoctorAppointmentId);


            
                  
        }
    }
}
