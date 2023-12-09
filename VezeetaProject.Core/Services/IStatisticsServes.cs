using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaProject.Core.Dtos.StasticDtos;
using VezeetaProject.Core.Dtos.StastisticDtos;
using VezeetaProject.Core.Models;

namespace VezeetaProject.Core.Services
{
    public interface IStatisticsServes
    {
         Task<int> NumOfDoctors(SearchFilter filter); 
         Task<int> NumOfPatients(SearchFilter filter); 
         Task<RequestsCountRespone> NumOfRequests(SearchFilter filter);
         Task<List<RequestCount>> TopSpecializations(int Top);
         Task<List<DoctorCountDto>> TopDoctors(int Top);

    }
}
