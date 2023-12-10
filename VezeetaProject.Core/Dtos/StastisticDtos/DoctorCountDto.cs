using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Dtos.StastisticDtos
{
    public class DoctorCountDto
    {
        public Byte[]  Image { get; set; }
        public string name { get; set; }
        public string specialize { get; set; }
        public int count { get; set; }
    }
}
