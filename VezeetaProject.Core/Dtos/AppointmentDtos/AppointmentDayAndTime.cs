
using System.Text.Json.Serialization;

namespace VezeetaProject.Core.Dtos.AppointmentDtos
{
    public class AppointmentDayAndTime
    {
        [Required]
        public Days day { get; set; }
      
        [Required]
        public List<TimeOnly> Times { get; set; }
    
    }
}
