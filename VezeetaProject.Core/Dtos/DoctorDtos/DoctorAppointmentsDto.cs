﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Dtos.DoctorDtos
{
    public class DoctorAppointmentsDto
    {
        public DoctorResponseDto doctorResponseDto { get; set; }

        public List<AppointmentDayAndTime> appointmentDayAndTimes { get; set; }
    }
}
