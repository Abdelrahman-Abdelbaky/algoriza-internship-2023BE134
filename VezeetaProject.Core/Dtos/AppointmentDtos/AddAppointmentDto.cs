using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Dtos.AppointmentDtos
{
    public class AddAppointmentDto
    {
        [Required]
        public decimal price { get; set; }
        [Required]
        public List<AppointmentDayAndTime> Appointment { get; set; }
       

    }
}
