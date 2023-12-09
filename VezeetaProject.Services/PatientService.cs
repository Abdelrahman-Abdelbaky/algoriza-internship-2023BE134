using System.Collections.Generic;
using VezeetaProject.Core.Consts;
using VezeetaProject.Core.Dtos.PatientDtos;

namespace VezeetaProject.Services
{
    public class PatientService : IpatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResources> localizer, IMapper mapper , UserManager<ApplicationUser> userManager )
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _mapper = mapper;
            _userManager = userManager;
        }
        /// <summary>
        /// make pagination 
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <param name="Search"></param>
        /// <returns></returns>
        public async Task<ResultDto<List<PatientDto>>> GeTAllPatient(int Page, int PageSize, string Search)
        {
            var result = new ResultDto<List<PatientDto>>();
            
            try 
            {
                Expression<Func<ApplicationUser, bool>> expression = x =>(x.Id.ToLower().Contains(Search.ToLower())
                                                                         |x.FirstName.ToLower().Contains(Search.ToLower())
                                                                         |x.LastName.ToLower().Contains(Search.ToLower())
                                                                         |x.PhoneNumber.ToLower().Contains(Search.ToLower())
                                                                         |x.Email.ToLower().Contains(Search.ToLower())
                                                                         |x.UserName.ToLower().Contains(Search.ToLower()));

                if(Search.ToLower() == "male")
                {
                    expression = x => (x.Id.ToLower().Contains(Search.ToLower())
                                                                         | x.FirstName.ToLower().Contains(Search.ToLower())
                                                                         | x.LastName.ToLower().Contains(Search.ToLower())
                                                                         | x.PhoneNumber.ToLower().Contains(Search.ToLower())
                                                                         | x.Email.ToLower().Contains(Search.ToLower())
                                                                         | x.UserName.ToLower().Contains(Search.ToLower())
                                                                         | x.Gender == Gender.Male);
                }

                if (Search.ToLower() == "female")
                {
                    expression = x => (x.Id.ToLower().Contains(Search.ToLower())
                                                                         | x.FirstName.ToLower().Contains(Search.ToLower())
                                                                         | x.LastName.ToLower().Contains(Search.ToLower())
                                                                         | x.PhoneNumber.ToLower().Contains(Search.ToLower())
                                                                         | x.Email.ToLower().Contains(Search.ToLower())
                                                                         | x.UserName.ToLower().Contains(Search.ToLower())
                                                                         | x.Gender == Gender.Female);
                }


                var patients = await _unitOfWork.applicationUser.SearchAsync(expression, PageSize, Page);

                if (patients is not null)
                {

                    List<PatientDto> list = new List<PatientDto>();

                    if (result is not null)
                        foreach (var item in patients)
                        {  
                            if (await _userManager.IsInRoleAsync(item, Roles.Patient.ToString()))
                            {
                                var patient = _mapper.Map<PatientDto>(item);
                                patient.Gender = (_localizer[ResourceItem.Language] == "English") ? item.Gender.ToString()
                                                                                                   : LanguageConverterServices.GenderFromEnglishToArabic((int)item.Gender);
                                list.Add(patient);
                            }
                        }

                    result.IsDone = true;
                    result.Object = list;
                }
                else {

                    result.ErrorMassage = _localizer[ResourceItem.NotFound];
                    return result;
                }
            }
            finally
            {
                
                _unitOfWork.Dispose();
            }


            return result;
        }

        public async Task<ResultDto<PatientWitRequestsDto>> GeTBYId(string Id)
        {
            var result = new ResultDto<PatientWitRequestsDto>();
            result.Object = new PatientWitRequestsDto();
            try 
            {
                var Patient = await _unitOfWork.applicationUser.FindAsync(x => x.Id == Id );

                
                if (Patient is not null && await _userManager.IsInRoleAsync(Patient, Roles.Patient.ToString()))
                {
                     result.Object.Patient = _mapper.Map<PatientDto>(Patient);
                     result.Object.Patient.Gender = (_localizer[ResourceItem.Language] == "English") ? Patient.Gender.ToString()
                                                                                       : LanguageConverterServices.GenderFromEnglishToArabic((int) Patient.Gender);
                    var BooKings = await _unitOfWork.Bookings.SearchAsync(x => x.PatientId == Patient.Id, new[] { "AppointmentTimes" } ); ;
                    if (BooKings is not null)
                    {

                        foreach (var booking in BooKings)
                        {
                            var doctorAppointment = await _unitOfWork.DoctorAppointments.FindAsync(x => x.Id == booking.AppointmentTimes.DoctorAppointmentId );
                            
                            var Doctor = await _unitOfWork.doctors.FindAsync(x => x.Id == booking.DoctorId, new[] { "Specialization", "ApplicationUser" });
                            
                            PatientRequestsDto request = _mapper.Map<PatientRequestsDto>(booking);
                        
                            request.day = (_localizer[ResourceItem.Language] == "English") ? doctorAppointment.Day.ToString()

                                                                                         : LanguageConverterServices.WeeKDaysFromEnglishToArabic((int)doctorAppointment.Day);


                            request.specialize = (_localizer[ResourceItem.Language] == "English") ? Doctor.Specialization.SpecializaEn : Doctor.Specialization.SpecializaAr;

                            request.status = (_localizer[ResourceItem.Language] == "English") ? booking.RequestStauts.ToString() : LanguageConverterServices.StatusFromEnglishToArabic((int)booking.RequestStauts);
                            request.DoctorName = Doctor.ApplicationUser.FirstName + " " + Doctor.ApplicationUser.LastName;

                            request.image = Doctor.ApplicationUser.Image;

                            result.Object.requests.Add(request);
                        }

                    }



                }
                else
                 result.ErrorMassage = _localizer[ResourceItem.NotFound];
              
                
                
            
            }
            finally
            { 
            
               _unitOfWork.Dispose();
            }

            return result;
        }
  
    
    }
}
