namespace VezeetaProject.Core.Models.Appointment
{
    public class DoctorAppointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Days Day { get; set; }
        public List<AppointmentTimes> appointmentTimes { get; set; }
     

    }
}
