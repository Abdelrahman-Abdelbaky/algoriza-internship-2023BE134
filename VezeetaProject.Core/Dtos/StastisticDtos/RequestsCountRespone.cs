using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Dtos.StasticDtos
{
    public class RequestsCountRespone
    {
       public RequestCount Requests { get; set; }
       public RequestCount PendingRequests { get; set; }
       public RequestCount completedRequests { get; set; }
       public RequestCount CancelledRequests { get; set; }
         
    }
}
