namespace VezeetaProject.Core.EntitysConfiguration
{
    public class DoctorEntityConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price)
                   .IsRequired(false);

            builder.Property(x => x.SpecializationId) 
                   .IsRequired();

            builder.Property(x => x.ApplicationUserId).
                    IsRequired();

            builder.HasMany(p => p.DoctorAppointment)
                   .WithOne(p => p.Doctor)
                   .HasForeignKey(p => p.DoctorId);

              builder.HasMany(p => p.Bookings)
                   .WithOne(p => p.Doctor)
                   .HasForeignKey(p => p.DoctorId).OnDelete(DeleteBehavior.NoAction);

            
        }
    }
}
