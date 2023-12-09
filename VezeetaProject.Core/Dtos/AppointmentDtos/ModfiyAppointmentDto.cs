using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaProject.Core.Consts;

namespace VezeetaProject.Core.Dtos.AppointmentDtos
{
    public class ModfiyAppointmentDto
    {
        [Required]
        public int OldTimeId { get; set; }
        [Required]
        public TimeOnly time { get; set; }
       
    }
}
