using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaProject.Core.Consts;

namespace VezeetaProject.Core.Models
{
    public class Discount
    {
        public int Id { get; set; }

        public string DiscountCode { get; set; }
      
        public int requsetCompleted { get; set; }

        public DiscountType DiscountType { get; set; }

        public decimal Value { get; set; }

        public bool IsActivate { get; set; } = true;

        public List<Booking> bookings { get; set; } 
    }
}
