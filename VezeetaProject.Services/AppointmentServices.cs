using VezeetaProject.Core.Dtos.AppointmentDtos;

namespace VezeetaProject.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        public AppointmentServices(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        /// <summary>
        /// Add appointment in the system
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResultDto<DoctorAppointment>> AddAppointmentAsync(AddAppointmentDto model, string userId)
        {
            var result = new ResultDto<DoctorAppointment>();
            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var doctor = await _unitOfWork.doctors.FindAsync(x => x.ApplicationUserId == userId);

                if (doctor != null)
                {

                    #region update price 

                    if (doctor.Price == 0 || doctor.Price is null)
                    {
                        doctor.Price = model.price;
                        await _unitOfWork.doctors.UpdateAsync(doctor);
                        _unitOfWork.Commit();
                        result.IsDone = true;
                    }
                    else
                    {

                        result.ErrorMassage += " _ " + _localizer[ResourceItem.PriceUpdateError];
                    }
                    #endregion

                    #region Add DAYS 
                    foreach (var appointments in model.Appointment)
                    {

                        if (!_unitOfWork.DoctorAppointments.FindAny(x => x.DoctorId == doctor.Id && x.Day == appointments.day))
                        {
                            var DoctorAppointment = new DoctorAppointment()
                            {
                                Day = appointments.day,
                                DoctorId = doctor.Id
                            };

                            await _unitOfWork.DoctorAppointments.AddAsync(DoctorAppointment);
                            _unitOfWork.Commit();
                        }

                    }
                    #endregion

                    #region Add times 
                    foreach (var appointments in model.Appointment)
                    {

                        var day = await _unitOfWork.DoctorAppointments.FindAsync(x => x.Day == appointments.day && x.DoctorId == doctor.Id);

                        foreach (var times in appointments.Times)
                        {
                            var dateTime = new DateTime(0001, 1, 1, times.Hour, times.Minute, 00);
                            if (_unitOfWork.AppointmentTimes.FindAny(x => x.Time == dateTime && x.DoctorAppointmentId == day.Id))
                            {
                                if (_localizer[ResourceItem.Language] == "English")
                                    result.ErrorMassage += " _ " + string.Format(_localizer[ResourceItem.TimeError], day.Day, dateTime.Hour, dateTime.Minute);
                                else result.ErrorMassage += " _ " + string.Format(_localizer[ResourceItem.TimeError], LanguageConverterServices.WeeKDaysFromEnglishToArabic((int)day.Day), dateTime.Hour, dateTime.Minute);

                                continue;
                            }

                            await _unitOfWork.AppointmentTimes.AddAsync(new AppointmentTimes { DoctorAppointmentId = day.Id, Time = dateTime });
                            _unitOfWork.Commit();
                        }

                    }
                    #endregion

                    result.IsDone = true;
                }
                else
                {
                    result.IsDone = false;
                    result.ErrorMassage = _localizer[ResourceItem.NotFound];
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                result.IsDone = false;
                result.ErrorMassage = _localizer[ResourceItem.AddFailed];
                transaction.Rollback();
            }
            finally
            {

                transaction.Dispose();
                _unitOfWork.Dispose();
            }

            return result;
        }

        public async Task<ResultDto<UpdateTimeDto>> UpdateAppointmentAsync(ModfiyAppointmentDto model, string userId)
        {

            var result = new ResultDto<UpdateTimeDto>();


            try
            {
              
                    var NewTime = new DateTime(0001, 1, 1, model.time.Hour, model.time.Minute, 0);
                    var doctor = await _unitOfWork.doctors.FindAsync(x => x.ApplicationUserId == userId);
                    var AppointmentTime = await _unitOfWork.AppointmentTimes.FindAsync(x => x.Id == model.OldTimeId && x.DoctorAppointment.DoctorId == doctor.Id , new[] { "DoctorAppointment" } );
                    var NewAppointmentTime = await _unitOfWork.AppointmentTimes.FindAsync(x => x.Time == NewTime );

                    if (AppointmentTime is not null && NewAppointmentTime is null)
                    {
                        var AppointmentUsed = _unitOfWork.Bookings.FindAny(x => x.AppointmentTimesId == AppointmentTime.Id && (x.RequestStauts == RequestStatus.Pending || x.RequestStauts == RequestStatus.Completed));

                        if (!AppointmentUsed)
                        {
                            AppointmentTime.Time = NewTime;
                            await _unitOfWork.AppointmentTimes.UpdateAsync(AppointmentTime);
                            _unitOfWork.Commit();
                            result.IsDone = true;

                        }
                        else result.ErrorMassage = _localizer[ResourceItem.TimeIsUsed];


                    }
                    else result.ErrorMassage = (_localizer[ResourceItem.Language] == "English") ?
                             _localizer[ResourceItem.NotFound]
                           : _localizer[ResourceItem.NotFound];
               


            }
            catch (Exception ex)
            {
            }
            finally
            {
                _unitOfWork.Dispose();
            }


            return result;
        }

        public async Task<ResultDto<UpdateTimeDto>> DeleteAppointmentAsync(int id, string userId)
        {

            var result = new ResultDto<UpdateTimeDto>();


            try
            {
               
                var user = await _unitOfWork.doctors.FindAsync(x => x.ApplicationUserId == userId);
                var doctor = await _unitOfWork.doctors.FindAsync(x => x.ApplicationUserId == userId);

                var AppointmentTime = await _unitOfWork.AppointmentTimes.FindAsync(x => x.Id == id && x.DoctorAppointment.DoctorId == doctor.Id, new[] { "DoctorAppointment" });

                if (AppointmentTime is not null)
                {
                    var AppointmentUsed = _unitOfWork.Bookings.FindAny(x => x.AppointmentTimesId == AppointmentTime.Id && (x.RequestStauts == RequestStatus.Pending || x.RequestStauts == RequestStatus.Completed));

                    if (!AppointmentUsed)
                    {
                        _unitOfWork.AppointmentTimes.Delete(AppointmentTime);
                        _unitOfWork.Commit();

                        result.IsDone = true;

                    }
                    else
                        result.ErrorMassage = _localizer[ResourceItem.TimeIsUsed];
                }
                else
                    result.ErrorMassage = (_localizer[ResourceItem.Language] == "English") ?
                        _localizer[ResourceItem.NotFound]
                      : _localizer[ResourceItem.NotFound];


            }



            catch (Exception ex)
            {

            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return result;
        }

    }
}
