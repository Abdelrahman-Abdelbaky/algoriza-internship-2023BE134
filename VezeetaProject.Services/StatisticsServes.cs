using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaProject.Core.Dtos.StasticDtos;
using VezeetaProject.Core.Dtos.StastisticDtos;

namespace VezeetaProject.Services
{
    public class StatisticsServes : IStatisticsServes
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly  IStringLocalizer<SharedResources> _localizer;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatisticsServes(IUnitOfWork unitOfWork, IStringLocalizer<SharedResources> localizer, UserManager<ApplicationUser> userManager)
        {

            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _userManager = userManager;
        }

        public async Task<int> NumOfDoctors(SearchFilter filter)
        {
            var count = 0;
            try
            {
                if (filter == SearchFilter.none)
                {

                    count = await _unitOfWork.doctors.CountAsync(x => true);
                }
                else
                {
                    count = await _unitOfWork.doctors.CountAsync(x => x.ApplicationUser.TimeStamp >= FilterTime((int)filter), new[] { "ApplicationUser" });
                }

            }
            finally { _unitOfWork.Dispose(); }
            return count;
        }

        public async Task<int> NumOfPatients(SearchFilter filter)
        {
            var count = 0;
            try
            {
                
                if ((int)filter == 0)
                {
                    var patients = await _unitOfWork.applicationUser.SearchAsync(x => true);
                    List<ApplicationUser> list = new List<ApplicationUser>();
                    foreach (var patient in patients)
                    {
                        if (await _userManager.IsInRoleAsync(patient, "Patient"))
                        {
                            list.Add(patient);
                        }

                    }
                    count = list.Count;
                }
                else
                {
                    var patients = await _unitOfWork.applicationUser.SearchAsync(x => x.TimeStamp >= FilterTime((int)filter));

                    List<ApplicationUser> list = new List<ApplicationUser>();
                    foreach (var patient in patients)
                    {
                        if (await _userManager.IsInRoleAsync(patient, "Patient"))
                        {
                            list.Add(patient);
                        }

                    }
                    count = list.Count;
                }
            }finally { _unitOfWork.Dispose(); }
            return count;
        }

        public async Task<RequestsCountRespone> NumOfRequests(SearchFilter filter)
        {
            RequestsCountRespone requestsCountRespone = new RequestsCountRespone();

            var count = 0;
            try
            {
                if ((int)filter == 0)
                {

                    count = await _unitOfWork.Bookings.CountAsync(x => true);
                    var Requests = await _unitOfWork.Bookings.GroupBy(x => x.RequestStauts, x => true);
                    var dictionary = Requests.ToDictionary(x => x.Key, x => x.Count());
                    requestsCountRespone.Requests = new RequestCount();
                    requestsCountRespone.Requests.name = "Total requests";
                    requestsCountRespone.Requests.count = count;
                    
                    requestsCountRespone.PendingRequests = new RequestCount();
                    requestsCountRespone.completedRequests = new RequestCount();
                    requestsCountRespone.CancelledRequests = new RequestCount();
                   
                    requestsCountRespone.PendingRequests.name = RequestStatus.Pending.ToString();
                    requestsCountRespone.completedRequests.name = RequestStatus.Completed.ToString();
                    requestsCountRespone.CancelledRequests.name = RequestStatus.Cancelled.ToString();

                    foreach (var item in dictionary)
                    {
                        if (item.Key == RequestStatus.Pending)
                        {

                            requestsCountRespone.PendingRequests.count = item.Value;
                        }
                        if (item.Key == RequestStatus.Completed)
                        {

                            requestsCountRespone.completedRequests.count = item.Value;
                        }
                        if (item.Key == RequestStatus.Cancelled)
                        {

                            requestsCountRespone.CancelledRequests.count = item.Value;
                        }

                    }
                }
                else
                {
                    count = await _unitOfWork.Bookings.CountAsync(x => x.TimeStamp >= FilterTime((int)filter));
                    var Requests = await _unitOfWork.Bookings.GroupBy(x => x.RequestStauts, x => x.TimeStamp >= FilterTime((int)filter));
                    var dictionary = Requests.ToDictionary(x => x.Key, x => x.Count());
                    requestsCountRespone.Requests = new RequestCount();
                    requestsCountRespone.Requests.name = "Total requests";
                    requestsCountRespone.Requests.count = count;
                    requestsCountRespone.PendingRequests = new RequestCount();
                    requestsCountRespone.completedRequests = new RequestCount();
                    requestsCountRespone.CancelledRequests = new RequestCount();
                    requestsCountRespone.PendingRequests.name = RequestStatus.Pending.ToString();
                    requestsCountRespone.completedRequests.name = RequestStatus.Completed.ToString();
                    requestsCountRespone.CancelledRequests.name = RequestStatus.Cancelled.ToString();

                    foreach (var item in dictionary)
                    {
                        if (item.Key == RequestStatus.Pending)
                        {


                            requestsCountRespone.PendingRequests.count = item.Value;
                        }
                        if (item.Key == RequestStatus.Completed)
                        {

                            requestsCountRespone.completedRequests.count = item.Value;
                        }
                        if (item.Key == RequestStatus.Cancelled)
                        {

                            requestsCountRespone.CancelledRequests.count = item.Value;
                        }

                    }
                }
            }
            finally { _unitOfWork.Dispose(); }
            return requestsCountRespone;
        }

        private DateTime FilterTime (int filter)
        {
            var dateTime = filter switch
            {
                1 => DateTime.Now.AddHours(-24),
                2 => DateTime.Now.AddDays(-7),
                3 => DateTime.Now.AddMonths(-1),
                4 => DateTime.Now.AddYears(-1),
            };
        
            return dateTime;
        }

        public async Task<List<RequestCount>> TopSpecializations( int Top)
        {
            var RequestCount = new List<RequestCount>();
            try
            {
                var Requests = await _unitOfWork.Bookings.GroupBy(x => x.Doctor, x => true, new[] { "Doctor" });
                var dictionary = Requests.ToDictionary(x => x.Key, x => x.Count());
                var Order = dictionary.OrderByDescending(x => x.Value).Take(Top);
              
                foreach (var item in Order)
                {
                    var specialize = await _unitOfWork.Specializations.FindAsync(x => x.Id == item.Key.SpecializationId);
                    RequestCount.Add(new RequestCount() {
                        name = (_localizer[ResourceItem.Language] == "English") ? specialize.SpecializaEn : specialize.SpecializaAr 
                      , count = item.Value
                    }) ; 
                   
                }

            }
            finally { _unitOfWork.Dispose();}

            return RequestCount;
        }


        public async Task<List<DoctorCountDto>> TopDoctors(int Top)
        {
            var RequestCount = new List<DoctorCountDto>();
            try
            {
                var Requests = await _unitOfWork.Bookings.GroupBy(x => x.Doctor, x => true, new[] { "Doctor" });
                var dictionary = Requests.ToDictionary(x => x.Key, x => x.Count());
                var Order = dictionary.OrderByDescending(x => x.Value).Take(Top);

                foreach (var item in Order)
                {
                    var specialize = await _unitOfWork.Specializations.FindAsync(x => x.Id == item.Key.SpecializationId);
                    var applicationUser = await _unitOfWork.applicationUser.FindAsync(x => x.Id == item.Key.ApplicationUserId);

                    RequestCount.Add(new DoctorCountDto()
                    {
                        name = applicationUser.FirstName + " " + applicationUser.LastName,
                        Image = applicationUser.Image,
                        specialize = (_localizer[ResourceItem.Language] == "English") ? specialize.SpecializaEn : specialize.SpecializaAr
                        , count = item.Value
                    });

                }

            }
            finally { _unitOfWork.Dispose(); }

            return RequestCount;
        }
    }
}
