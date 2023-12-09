using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Dtos.Booking
{
    public class AddBookingDto
    {
        [Required]
        public int TimeId { get; set; }

        public string discountCode { get; set; }
    }
}
