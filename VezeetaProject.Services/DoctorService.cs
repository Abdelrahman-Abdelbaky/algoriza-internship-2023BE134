using VezeetaProject.Core.Dtos.AuthenticationDtos;
using VezeetaProject.Core.Dtos.Booking;
using VezeetaProject.Core.Models.Users;

namespace VezeetaProject.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly Core.Services.IMailService _mailService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IImageService _imageService;
       

        public DoctorService(IUnitOfWork unitOfWork,IMapper mappe, IAuthService authService, Core.Services.IMailService mailService, IStringLocalizer<SharedResources> localizer,IImageService ImageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mappe;
            _authService = authService;
            _mailService = mailService;
            _localizer = localizer;
            _imageService = ImageService;
        }
        /// <summary>
        /// Add doctor in the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultDto<Doctor></returns>
        public async Task<ResultDto<Doctor>> AddDoctor(DoctorDto model)
        {
            ResultDto<Doctor> resultDto = new ResultDto<Doctor>();
            Doctor doctor = null;
            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                AuthDto authDto = null;
                var registerDto = _mapper.Map<RegisterDto>(model); 
                registerDto.Password = CreatePassword();

                if (registerDto != null && _unitOfWork.Specializations.FindAny(Specialization => Specialization.Id == model.specializeId))
                    authDto = await _authService.RegisterAsync(registerDto, Roles.Doctor.ToString());
                else resultDto.ErrorMassage = _localizer[ResourceItem.specializeIdIsWrong];
           
                if (authDto.IsAuthenticated)
                    doctor = await _unitOfWork.doctors.AddAsync( new Doctor {ApplicationUserId = authDto.userId,
                                                                             SpecializationId = model.specializeId 
                                                                         
                    });
                else resultDto.ErrorMassage +=(authDto.Message is  null)? "":authDto.Message;

                if (doctor is not null)
                {
                    await _mailService.MailSender(model.Email,
                                "Welcome To vezeeta system",
                                $"your Email {registerDto.Email} your password {registerDto.Password}");
                    _unitOfWork.Commit();
                    resultDto.IsDone = true;
                    resultDto.Object = doctor;
                }
                else {
                    resultDto.IsDone = false;
                    resultDto.ErrorMassage += _localizer[ResourceItem.AddFailed];
                   }
              
                transaction.Commit();
            }
            catch (Exception ex)
            {
                resultDto.IsDone = false;
                resultDto.ErrorMassage += _localizer[ResourceItem.AddFailed];
                transaction.Rollback();
                if (doctor is not null)
                {
                   var application = await _unitOfWork.applicationUser.FindAsync(x => x.Id == doctor.ApplicationUserId);
                   if(application != null) 
                      _unitOfWork.applicationUser.Delete(application);
                }
            }
            finally
            { 
             transaction.Dispose();
             _unitOfWork.Dispose(); 
            }
            return resultDto;
        }

     

        public async Task<ResultDto<DoctorResponseDto>> GetDoctorById(int Id)
        {
            ResultDto<DoctorResponseDto> result = new ResultDto<DoctorResponseDto>();
            try
            {
               
                var Doctor = await _unitOfWork.doctors.FindAsync(p => p.Id == Id, new[] { "ApplicationUser" , "Specialization" });
                if (Doctor is null || Doctor.ApplicationUser is null)
                {
                    result.IsDone = false;
                    result.ErrorMassage = _localizer[ResourceItem.NotFound];
                }
                else
                {
                    
                    result.IsDone = true;
                    result.Object = _mapper.Map<DoctorResponseDto>(Doctor);
                    result.Object.specialization = (_localizer[ResourceItem.Language] == "English") ? Doctor.Specialization.SpecializaEn : Doctor.Specialization.SpecializaAr;
                    result.Object.Gender = (_localizer[ResourceItem.Language] == "English") ? Doctor.ApplicationUser.Gender.ToString() : LanguageConverterServices.GenderFromEnglishToArabic((int)Doctor.ApplicationUser.Gender);
                    var specialization =await _unitOfWork.Specializations.GetbyIdAsync(Doctor.SpecializationId);
                    result.Object.specialization =  (_localizer[ResourceItem.Language] == "English") ? specialization.SpecializaEn : specialization.SpecializaAr;
                    result.Object.DateOfBirth = Doctor.ApplicationUser.DateOfBirth.ToString("yyyy/MM/dd");
                }
            }catch (Exception ex)
            {
                result.IsDone = false;
                result.ErrorMassage = _localizer[ResourceItem.NotFound];
            }
            finally 
            {
                _unitOfWork.Dispose();
            }
            
            return result;
        }
        /// <summary>
        /// create random password 
        /// </summary>
        /// <returns>password string type </returns>
        private string CreatePassword()
        {
            const string SmallLetters = "abcdefghijklmnopqrstuvwxyz";
            const string CapitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "123456789";
            const string SpecialChar = "!@#$%^&*()_";
            Random random = new Random();
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < 2; i++)
            {
                password.Append(CapitalLetters[random.Next(CapitalLetters.Length)]);
            }
            for (int i = 0; i < 2; i++)
            {
                password.Append(SmallLetters[random.Next(SmallLetters.Length)]);
            }
            for (int i = 0; i < 1; i++)
            {
                password.Append(SpecialChar[random.Next(SpecialChar.Length)]);
            }
            for (int i = 0; i < 4; i++)
            {
                password.Append(numbers[random.Next(numbers.Length)]);
            }

            return password.ToString();

        }
        /// <summary>
        /// Delete doctor from system
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>ResultDto<Doctor></Doctor></returns>
        public async Task<ResultDto<Doctor>> DeleteDoctor(int Id)
        {
            var result = new ResultDto<Doctor>();
            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var doctor = await _unitOfWork.doctors.GetbyIdAsync(Id);
                if (doctor is not null)
                {
                 
                  var requests =  _unitOfWork.Bookings.FindAny(x => x.DoctorId == doctor.Id && x.RequestStauts != RequestStatus.Pending, new[]{ "AppointmentTimes" });

                    if (!requests)
                    {
                        var user = await _unitOfWork.applicationUser.FindAsync(x => x.Id == doctor.ApplicationUserId);


                        _unitOfWork.doctors.Delete(doctor);
                        _unitOfWork.Commit();

                        _unitOfWork.applicationUser.Delete(user);
                        _unitOfWork.Commit();

                        transaction.Commit();
                        result.IsDone = true;
                        result.Object = doctor;
                    }
                    else {

                        result.ErrorMassage = _localizer[ResourceItem.doctorHasBookings];
                    }
                }
                else 
                {
                    result.IsDone = false;
                    result.ErrorMassage = _localizer[ResourceItem.NotFound];
                    transaction.Dispose();
                }

            }
            catch
            (Exception ex)
            {
                result.IsDone = false;
                result.ErrorMassage = _localizer[ResourceItem.DeleteFailed];
                transaction.Rollback();
               
            }
            finally 
            {
                transaction.Dispose();
                _unitOfWork.Dispose();
            }

            return result;
        }
        /// <summary>
        /// update doctor 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultDto<Doctor></returns>
        public async Task<ResultDto<Doctor>> UpdateDoctor(UpdateDoctorDto model)
        {
            var transaction = _unitOfWork.BeginTransaction();   
            var result = new ResultDto<Doctor>();
            try
            {
                var Doctor = await _unitOfWork.doctors.FindAsync(x =>x.Id == model.Id);
                var specialization = await _unitOfWork.Specializations.FindAsync(x=>x.Id== model.specializeId);
               
                if (Doctor is not null && specialization is not null)
                {
                    var newDoctor= _mapper.Map<Doctor>(model);
                    newDoctor.ApplicationUserId = Doctor.ApplicationUserId;
                  
                    newDoctor.Price = Doctor.Price;   
                    newDoctor.SpecializationId = model.specializeId;
                    
                    var newUser = await _unitOfWork.applicationUser.FindAsync(x => x.Id == Doctor.ApplicationUserId);

                    if (newUser.Email == model.Email || !_unitOfWork.applicationUser.FindAny(x => x.Email == model.Email))
                    {
                        newUser.FirstName = model.FirstName;
                        newUser.LastName = model.LastName;
                        newUser.Email = model.Email;
                        newUser.Gender = model.Gender;
                        newUser.DateOfBirth = model.DateOfBirth;
                        newUser.PhoneNumber = model.phone;
                        newUser.Image = await _imageService.EncodeImageAsync(model.Image);
                        newUser.UserName = model.Email;


                        await _unitOfWork.applicationUser.UpdateAsync(newUser);
                        _unitOfWork.Commit();

                        await _unitOfWork.doctors.UpdateAsync(newDoctor);
                        _unitOfWork.Commit();

                        result.IsDone = true;
                        result.Object = newDoctor;
                    }else result.ErrorMassage = _localizer[ResourceItem.EmailUsed].ToString();

                }
                else 
                {
                    result.ErrorMassage = _localizer[ResourceItem.NotFound].ToString();

                }

                transaction.Commit();
                
            }
            catch (Exception ex)
            {
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

        public async Task<List<DoctorResponseDto>> GetAllDoctor(int page, int pageSize, string search)
        {
            List<DoctorResponseDto> list = new List<DoctorResponseDto>();
            try
            {
                Expression<Func<Doctor, bool>> expression = (x => x.ApplicationUser.FirstName.ToLower().Contains(search.ToLower())
                                                  | x.ApplicationUser.LastName.ToLower().Contains(search.ToLower())
                                                  | x.ApplicationUser.Email.ToLower().Contains(search.ToLower())
                                                  | x.ApplicationUser.PhoneNumber.ToLower().Contains(search.ToLower())
                                                  | x.Specialization.SpecializaEn.ToLower().Contains(search.ToLower())
                                                  | x.Specialization.SpecializaAr.ToLower().Contains(search.ToLower())
                                                  );

                if (search.ToLower().Contains("male"))
                {

                    expression = (x => x.ApplicationUser.Gender == Gender.Male
                                                                 | x.ApplicationUser.FirstName.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.LastName.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.Email.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.PhoneNumber.ToLower().Contains(search.ToLower())
                                                                 | x.Specialization.SpecializaEn.ToLower().Contains(search.ToLower())
                                                                 | x.Specialization.SpecializaAr.ToLower().Contains(search.ToLower())
                                                                 );

                }

                if (search.ToLower().Contains("female"))
                {

                    expression = (x => x.ApplicationUser.Gender == Gender.Female
                                                                 | x.ApplicationUser.FirstName.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.LastName.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.Email.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.PhoneNumber.ToLower().Contains(search.ToLower())
                                                                 | x.Specialization.SpecializaEn.ToLower().Contains(search.ToLower())
                                                                 | x.Specialization.SpecializaAr.ToLower().Contains(search.ToLower())
                                                                 );

                }


                var result = await _unitOfWork.doctors.SearchAsync(expression, pageSize, page, new[] { "ApplicationUser", "Specialization" });



                if (result is not null)
                    foreach (var item in result)
                    {
                        var doctor = _mapper.Map<DoctorResponseDto>(item);
                        doctor.specialization = (_localizer[ResourceItem.Language] == "English") ? item.Specialization.SpecializaEn : item.Specialization.SpecializaAr;
                        doctor.Gender = (_localizer[ResourceItem.Language] == "English") ? item.ApplicationUser.Gender.ToString() : LanguageConverterServices.GenderFromEnglishToArabic((int)item.ApplicationUser.Gender);
                        list.Add(doctor);
                    }
                else {

                    return null;
                }
            }
            finally { _unitOfWork.Dispose(); }
            return list;
        }


        public async Task<List<DoctorAppointmentsDto2>> GetAllDoctorWithAppointment(int page, int pageSize, string search)
        {
            List<DoctorAppointmentsDto2> list = new List<DoctorAppointmentsDto2>();
            try
            {
                Expression<Func<Doctor, bool>> expression = (x => x.ApplicationUser.FirstName.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.LastName.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.Email.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.PhoneNumber.ToLower().Contains(search.ToLower())
                                                                 | x.Specialization.SpecializaEn.ToLower().Contains(search.ToLower())
                                                                 | x.Specialization.SpecializaAr.ToLower().Contains(search.ToLower())                                                
                                                                 );
                if (search.ToLower().Contains("male")) {

                    expression = (x => x.ApplicationUser.Gender == Gender.Male                          
                                                                 | x.ApplicationUser.FirstName.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.LastName.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.Email.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.PhoneNumber.ToLower().Contains(search.ToLower())
                                                                 | x.Specialization.SpecializaEn.ToLower().Contains(search.ToLower())
                                                                 | x.Specialization.SpecializaAr.ToLower().Contains(search.ToLower())
                                                                 );

                }
                                                          
                if (search.ToLower().Contains("female"))
                {

                    expression = (x => x.ApplicationUser.Gender == Gender.Female
                                                                 | x.ApplicationUser.FirstName.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.LastName.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.Email.ToLower().Contains(search.ToLower())
                                                                 | x.ApplicationUser.PhoneNumber.ToLower().Contains(search.ToLower())
                                                                 | x.Specialization.SpecializaEn.ToLower().Contains(search.ToLower())
                                                                 | x.Specialization.SpecializaAr.ToLower().Contains(search.ToLower())
                                                                 );

                }



                var result = await _unitOfWork.doctors.SearchAsync(expression, pageSize, page, new[] { "ApplicationUser", "Specialization", "DoctorAppointment" });



                if (result is not null)
                    foreach (var item in result)
                    {
                        var doctor = new DoctorAppointmentsDto2(){};
                        var doctorResponseDto = new DoctorResponseDto();
                         doctorResponseDto.Fullname = (item.ApplicationUser.FirstName + " " + item.ApplicationUser.LastName );
                         doctorResponseDto.Image = item.ApplicationUser.Image;
                         doctorResponseDto.phone = item.ApplicationUser.PhoneNumber;
                         doctorResponseDto.Email = item.ApplicationUser.Email;
                         doctorResponseDto.DateOfBirth = item.ApplicationUser.DateOfBirth.ToString("MM/dd/yyyy");

                        doctorResponseDto.specialization = (_localizer[ResourceItem.Language] == "English") ? item.Specialization.SpecializaEn : item.Specialization.SpecializaAr;
                        doctorResponseDto.Gender = (_localizer[ResourceItem.Language] == "English") ? item.ApplicationUser.Gender.ToString() : LanguageConverterServices.GenderFromEnglishToArabic((int)item.ApplicationUser.Gender);
                        doctor.doctorResponseDto = doctorResponseDto;
                        doctor.appointmentDayAndTimes = new List<AppointmentDayAndTime2>();
                        foreach (var day in item.DoctorAppointment) 
                        {
                            var Day = (_localizer[ResourceItem.Language] == "English") ? day.Day.ToString()
                                                                                      : LanguageConverterServices.WeeKDaysFromEnglishToArabic((int)day.Day);
                            var times = await _unitOfWork.AppointmentTimes.SearchAsync(x=> x.DoctorAppointmentId == day.Id);
                            var Times = new List<TimeAndId>();
                            foreach (var time in times)
                            {
                                Times.Add(new TimeAndId {Id = time.Id, Time = time.Time.ToString("hh:mm") });
                            }

                            var appointmentDayAndTimes2 = new AppointmentDayAndTime2() { day = Day,  Times =Times};
                     
                            doctor.appointmentDayAndTimes.Add(appointmentDayAndTimes2);
                        }
                        list.Add(doctor);
                    }
                else
                {

                    return null;
                }
            }
            finally { _unitOfWork.Dispose(); }
            return list;
        }
    
    
    }
}
