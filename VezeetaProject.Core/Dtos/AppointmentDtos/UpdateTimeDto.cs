using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Dtos.AppointmentDtos
{
    public class UpdateTimeDto:ModfiyAppointmentDto
    {
        [Required]
        public TimeOnly newTime { get; set; }
    }
}
