using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaProject.Core.Dtos.Booking;

namespace VezeetaProject.Core.Dtos.AppointmentDtos
{
    public class AppointmentDayAndTime2
    {
        [Required]
        public string day { get; set; }

        [Required]
        public List<TimeAndId> Times { get; set; }
    }
}
