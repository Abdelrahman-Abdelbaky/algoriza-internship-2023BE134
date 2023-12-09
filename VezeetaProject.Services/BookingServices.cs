using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaProject.Core.Dtos.Booking;
using VezeetaProject.Core.Dtos.PatientDtos;
using VezeetaProject.Core.Models;

namespace VezeetaProject.Services
{
    public class BooKingServices : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;

        public BooKingServices(IUnitOfWork unitOfWork, IStringLocalizer<SharedResources> localizer , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<ResultDto<AddBookingDto>> Booking(AddBookingDto model, string id)
        {
            var result = new ResultDto<AddBookingDto>();
            try
            {
                var time = await _unitOfWork.AppointmentTimes.FindAsync(x => x.Id == model.TimeId, new[] { "DoctorAppointment" });
         
                


                if (time is not null && ! _unitOfWork.Bookings.FindAny(x => x.AppointmentTimesId == time.Id && x.RequestStauts == RequestStatus.Pending))
                {
                    var doctor = await _unitOfWork.doctors.FindAsync(x => x.Id == time.DoctorAppointment.DoctorId);

                    if (_unitOfWork.Bookings.FindAny(x => x.PatientId == id && x.DoctorId == doctor.Id && x.RequestStauts == RequestStatus.Pending))
                    {

                        result.ErrorMassage = _localizer[ResourceItem.DoctorBookingError];
                        return result;
                    }

                    if (string.IsNullOrEmpty(model.discountCode))
                    {
                        var Booking = new Booking
                        {
                            AppointmentTimesId = time.Id,
                            TimeStamp = DateTime.Now,
                            Price = (decimal)doctor.Price,
                            PatientId = id,
                            DoctorId = doctor.Id,
                            RequestStauts = RequestStatus.Pending,
                            FinalPrice = (decimal)doctor.Price
                           
                        };
                        await _unitOfWork.Bookings.AddAsync(Booking);
                        _unitOfWork.Commit();
                        result.IsDone = true;
                    }
                    else 
                    {
                       var discount = await _unitOfWork.Discounts.FindAsync(x => x.DiscountCode == model.discountCode);
                        if (discount is not null && discount.IsActivate)
                        {

                            if (await _unitOfWork.Bookings.CountAsync(x => x.PatientId == id && x.RequestStauts == RequestStatus.Completed) >= discount.requsetCompleted)
                            {
                                var Booking = new Booking
                                {
                                    AppointmentTimesId = time.Id,
                                    DiscountId = discount.Id,
                                    DoctorId = doctor.Id,
                                    TimeStamp = DateTime.Now,
                                    Price = (decimal)doctor.Price,
                                    PatientId = id,
                                    RequestStauts = RequestStatus.Pending,
                                 
                                    FinalPrice = CalcFinalPrice((decimal)doctor.Price, discount.DiscountType, discount.Value)
                                };
                                await _unitOfWork.Bookings.AddAsync(Booking);
                                _unitOfWork.Commit();
                                result.IsDone = true;
                            }
                            else
                            {
                               result.ErrorMassage = string.Format( _localizer[ResourceItem.RequestConditionError] , discount.requsetCompleted);
                            }
                        }
                        else
                        {
                            result.ErrorMassage = _localizer[ResourceItem.DiscountCodeNotFound];
                        }
                    }
                }
                else
                {
                    result.ErrorMassage = _localizer[ResourceItem.NotFound];
                }
            }
            finally {
                _unitOfWork.Dispose();
            }
            return result;
        }

        public async Task<ResultDto<Booking>> CancelBooking(int id)
        {
            var result = new ResultDto<Booking>();
            try
            {
                var booking  = await _unitOfWork.Bookings.FindAsync(x => x.Id == id && x.RequestStauts != RequestStatus.Completed);


                if (booking is null )
                {
                    result.ErrorMassage = _localizer[ResourceItem.NotFound];
                }
                else
                {
                    booking.RequestStauts = RequestStatus.Cancelled;

                    await _unitOfWork.Bookings.UpdateAsync(booking);
                    _unitOfWork.Commit();

                    result.IsDone = true;
                }
            }
            finally
            {
                _unitOfWork.Dispose();
            }
            return result;
        }

        public async Task<ResultDto<Booking>> ConfiromCheckUp(int Bookingid , string DocctorId) {
            var result = new ResultDto<Booking>();
         
            try
            {
                var doctor = await _unitOfWork.doctors.FindAsync(x => x.ApplicationUserId == DocctorId );
                var booking = await _unitOfWork.Bookings.FindAsync(x => x.Id == Bookingid && x.DoctorId == doctor.Id && x.RequestStauts == RequestStatus.Pending);


                if (booking is null)
                {
                    result.ErrorMassage = _localizer[ResourceItem.NotFound];
                }
                else
                {
                    booking.RequestStauts = RequestStatus.Completed;

                    await _unitOfWork.Bookings.UpdateAsync(booking);
                    _unitOfWork.Commit();

                    result.IsDone = true;
                }
            }
            finally
            {
                _unitOfWork.Dispose();
            }
            return result;

        }

        public Task<ResultDto<ResponeBookingDto>> GetAllBooking(ResponeBookingDto model, string id)
        {
            throw new NotImplementedException();
        }

        private decimal CalcFinalPrice(decimal price, DiscountType discountType, decimal discountAmount)
        {

            var finalPrice = 0;
            if (discountType == DiscountType.Value)
            {
                if (discountAmount >= price)
                    return 0;
                else
                    return price - discountAmount;
            }
            else
            {
                if (discountAmount == 100)
                    return 0;
                else
                    return ((100 - discountAmount) / 100) * price;
            }
        }

        public async Task<List<PatientbookingsDto>> GetAll(string PatientId)
        {
            var result = new List<PatientbookingsDto>();
            try
            {
              
                var bookings = await _unitOfWork.Bookings.SearchAsync(x => x.PatientId == PatientId,new[] {"AppointmentTimes", "Doctor","Discount" });


                if (bookings.Count() == 0)
                {

                }
                else
                {
                    foreach (var booking in bookings)
                    {
                        var doctor = await _unitOfWork.applicationUser.FindAsync(x => x.Id == booking.Doctor.ApplicationUserId, new[] { "Doctor" });
                        var specialization = await _unitOfWork.Specializations.FindAsync(x => x.Id == doctor.Doctor.SpecializationId);

                        var Patientbooking = _mapper.Map<PatientbookingsDto>(booking);
                        Patientbooking.image = doctor.Image;
                        Patientbooking.discoundCode = (booking.Discount is null)?"": booking.Discount.DiscountCode;
                        Patientbooking.docotorName = doctor.FirstName + " " + doctor.LastName;
                        var day = await _unitOfWork.DoctorAppointments.FindAsync(x => x.Id == booking.AppointmentTimes.DoctorAppointmentId);

                        Patientbooking.specialize = (_localizer[ResourceItem.Language] == "English") ? specialization.SpecializaEn
                                                                                                    : specialization.SpecializaAr;

                        Patientbooking.status = (_localizer[ResourceItem.Language] == "English") ? booking.RequestStauts.ToString()
                                                                                                 : LanguageConverterServices.StatusFromEnglishToArabic((int)booking.RequestStauts);

                        Patientbooking.day = (_localizer[ResourceItem.Language] == "English") ? day.Day.ToString()
                                                                                              : LanguageConverterServices.WeeKDaysFromEnglishToArabic((int)day.Day);
                        result.Add(Patientbooking);
                    }
                }
            }
            finally 
            {
                _unitOfWork.Dispose();
            }

            return result;
        }

        public async Task<List<PatientWithAppointmentDto>> GetAllBookingAppointmentForDoctor (int Page, int PageSize, Days Search, string userID)
        {
            List<PatientWithAppointmentDto> result = new List<PatientWithAppointmentDto>();
            try 
            {
             var doctor = await _unitOfWork.doctors.FindAsync(x => x.ApplicationUserId == userID);
             var appointment = await _unitOfWork.DoctorAppointments.FindAsync(x => x.Day == Search && x.DoctorId == doctor.Id);
            
                if (appointment is not null ) 
                {
                    var bookings = await _unitOfWork.Bookings.SearchAsync(x => x.AppointmentTimes.DoctorAppointmentId == appointment.Id && x.RequestStauts == RequestStatus.Pending , PageSize, Page, new []{ "AppointmentTimes" } );
                
                    foreach ( var booking in bookings)
                    {
                        var patient = await _unitOfWork.applicationUser.FindAsync(x => x.Id == booking.PatientId);
                        var PatientWithAppointmentDto = new PatientWithAppointmentDto()
                        {
                            PatientName = patient.FirstName + " " + patient.LastName,
                            Email = patient.Email,
                            Image = (patient.Image is null)? "": patient.Image.ToString(),
                            phone = patient.PhoneNumber,
                            age = CalcAge(patient.DateOfBirth),
                            Gender = (_localizer[ResourceItem.Language] == "English") ? patient.Gender.ToString() : LanguageConverterServices.GenderFromEnglishToArabic((int)patient.Gender),
                            Appointment =string.Format(_localizer[ResourceItem.Appointment], LanguageConverterServices.WeeKDaysFromEnglishToArabic((int)appointment.Day), booking.AppointmentTimes.Time.Hour, booking.AppointmentTimes.Time.Day)
                        };

                        result.Add(PatientWithAppointmentDto);

                    }
                }
            
            }
            finally { _unitOfWork.Dispose(); }
           return result;
 
        }

        private int CalcAge(DateTime date) {

            var today = DateTime.Today;

            var age = today.Year - date.Year;

            if (date.Date > today.AddYears(-age)) age--;

            return age;
        }
    }

}
