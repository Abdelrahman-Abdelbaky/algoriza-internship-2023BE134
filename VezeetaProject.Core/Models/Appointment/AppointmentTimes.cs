namespace VezeetaProject.Core.Models.Appointment
{
    public class AppointmentTimes
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int DoctorAppointmentId { get; set; }
        public DoctorAppointment DoctorAppointment { get; set; }
        public List<Booking> Booking { get; set; }

       
    }
}
