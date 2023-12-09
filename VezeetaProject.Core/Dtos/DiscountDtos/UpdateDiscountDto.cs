using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Dtos.DiscountDtos
{
    public class UpdateDiscountDto : DiscountDto
    {
        [Required]
        public int Id { get; set; }
    }
}
