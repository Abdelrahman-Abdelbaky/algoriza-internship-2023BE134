namespace VezeetaProject.Core.EntitysConfiguration
{
    public class IdentityRolesEntityCofiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
         
            builder.HasData
                (
                  new IdentityRole()
                  {
                      Id= DefaultIds.ROLE_ID,
                      Name= Roles.Admin,
                      ConcurrencyStamp=Guid.NewGuid().ToString(),
                      NormalizedName= Roles.Admin.ToUpper() 
                  },
                  new IdentityRole() {
                      Id = Guid.NewGuid().ToString(),
                      Name =Roles.Doctor,
                      ConcurrencyStamp=Guid.NewGuid().ToString(),
                      NormalizedName= Roles.Doctor.ToUpper()
                  },
                  new IdentityRole() {
                      Id = Guid.NewGuid().ToString(),
                      Name =Roles.Patient,
                      ConcurrencyStamp= Guid.NewGuid().ToString(),
                      NormalizedName= Roles.Patient.ToUpper()
                  }
                );
        }
    }
}
