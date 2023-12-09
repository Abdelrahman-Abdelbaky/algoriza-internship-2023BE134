using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Dtos.DoctorDtos
{
    public class DoctorAppointmentsDto2
    {
         public DoctorResponseDto doctorResponseDto { get; set; }

        public List<AppointmentDayAndTime2> appointmentDayAndTimes { get; set; }
    }
}
