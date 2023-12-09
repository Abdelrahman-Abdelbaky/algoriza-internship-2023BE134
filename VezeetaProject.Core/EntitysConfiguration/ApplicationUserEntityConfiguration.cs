using Microsoft.Extensions.Options;

namespace VezeetaProject.Core.EntitysConfiguration
{
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {

    

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(p => p.FirstName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(p => p.LastName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(p => p.Gender)
                   .IsRequired();

            builder.Property(p => p.DateOfBirth)
                   .IsRequired();

            builder.Property(p => p.Email).
                    IsRequired();

            builder.Property(p => p.PhoneNumber).
                    IsRequired();

            builder.Property(p => p.TimeStamp).
                 IsRequired();

            builder.HasOne(p => p.Doctor)
                   .WithOne(p => p.ApplicationUser)
                   .HasForeignKey<Doctor>(p => p.ApplicationUserId);

            builder.HasMany(p => p.Bookings)
                   .WithOne(p => p.Patient) 
                   .HasForeignKey(p => p.PatientId)
                   .OnDelete(DeleteBehavior.NoAction);
                   
            

      



            var DefualitAccount = new ApplicationUser()
            {
                Id = DefaultIds.ADMIN_ID,
                FirstName = "Admin",
                LastName = "Admin",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(2001,9,27),
                UserName = "Admin@gmail.com",
                Email = "Admin@gmail.com",
                PhoneNumber ="01280460742",
                NormalizedEmail= "Admin@gmail.com".ToUpper()
            };

            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            DefualitAccount.PasswordHash = ph.HashPassword(DefualitAccount, "Admin@123");
            
            builder.HasData(DefualitAccount);


         
        }
    }
}
