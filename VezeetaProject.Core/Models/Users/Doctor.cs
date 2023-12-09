namespace VezeetaProject.Core.Models.Users
{
    public class Doctor
    {
        public int Id { get; set; }
        public Decimal? Price { get; set; }
        public int SpecializationId { get; set; }
        public  Specialization Specialization { get; set; }
        public string ApplicationUserId { get; set; }
        public  ApplicationUser ApplicationUser { get;}
        public List<DoctorAppointment> DoctorAppointment { get; set; }
        public List<Booking> Bookings { get; set; }

       
        
    }
}
