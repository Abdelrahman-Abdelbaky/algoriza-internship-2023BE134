using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaProject.Core.Consts;
using VezeetaProject.Core.Models.Appointment;
using VezeetaProject.Core.Models.Users;

namespace VezeetaProject.Core.Models
{
    public class Booking
    {
        public int Id { get; set; } 
        public string PatientId { get; set;}
        public ApplicationUser Patient { get; set;}
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set;}
        public RequestStatus RequestStauts { get; set; }
        public int AppointmentTimesId { get; set; }
        public  AppointmentTimes AppointmentTimes { get; set; }
        public Discount Discount { get; set; }
        public int? DiscountId { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal FinalPrice { get; set;}
        public decimal Price { get; set; }

    }
}
