 namespace VezeetaProject.RepositoryEF
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new ApplicationUserEntityConfiguration().Configure(builder.Entity<ApplicationUser>());
            new DoctorEntityConfiguration().Configure(builder.Entity<Doctor>());
            new AppointmentTimesEntityConfiguration().Configure(builder.Entity<AppointmentTimes>());
            new DoctorAppointmentEntityConfiguration().Configure(builder.Entity<DoctorAppointment>());
            new DiscountEntityConfiguration().Configure(builder.Entity<Discount>());
            new BookingEntityConfiguration().Configure(builder.Entity<Booking>());
            new SpecializationEntityConfiguration().Configure(builder.Entity<Specialization>());
            new IdentityRolesEntityCofiguration().Configure(builder.Entity<IdentityRole>());
            new seedData().Configure(builder.Entity<IdentityUserRole<string>>());          

        }


        public DbSet<Specialization> Specializations { get; set; }
       
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<DoctorAppointment> doctorAppointments { get; set; }

     



    }
}
