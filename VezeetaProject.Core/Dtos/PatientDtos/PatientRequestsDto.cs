using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Dtos.PatientDtos
{
    public class PatientRequestsDto
    {
        
        [Required]
        public Byte[] image { get; set; }

        [Required]
        public string DoctorName { get; set; }

        [Required]
        public string specialize { get; set; }

        [Required]
        public string day { get; set;}

        [Required]
        public string time { get; set;}

        [Required]
        public Decimal price { get; set;}

        [Required]
        public string DiscoundCode { get; set;}

        [Required]
        public Decimal finalPrice { get; set; }

        [Required]
        public string status { get; set;}

    }
}
