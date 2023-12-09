using VezeetaProject.Core.Dtos.AuthenticationDtos;
using VezeetaProject.Core.Dtos.Booking;
using VezeetaProject.Core.Dtos.DiscountDtos;
using VezeetaProject.Core.Resources;

namespace VezeetaProject.Core
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DiscountDto, Discount>()
                   .ForMember(dest => dest.DiscountCode, src => src.MapFrom(src => src.DiscountCode))
                   .ForMember(dest => dest.requsetCompleted, src => src.MapFrom(src => src.requsetCompleted))
                   .ForMember(dest => dest.Value, src => src.MapFrom(src => src.Value))
                   .ForMember(dest => dest.DiscountType, src => src.MapFrom(src => src.DiscountType))
                   .ReverseMap();

            CreateMap<UpdateDiscountDto, Discount>()
                   .ForMember(dest => dest.Id, src => src.MapFrom(src => src.Id))
                   .ForMember(dest => dest.DiscountCode, src => src.MapFrom(src => src.DiscountCode))
                   .ForMember(dest => dest.requsetCompleted, src => src.MapFrom(src => src.requsetCompleted))
                   .ForMember(dest => dest.Value, src => src.MapFrom(src => src.Value))
                   .ForMember(dest => dest.DiscountType, src => src.MapFrom(src => src.DiscountType))
                   .ReverseMap();
            CreateMap<DoctorDto, RegisterDto>()
                    .ForMember(dest => dest.FirstName, src => src.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, src => src.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.Email, src => src.MapFrom(src => src.Email))
                    .ForMember(dest => dest.phone, src => src.MapFrom(src => src.phone))
                    .ForMember(dest => dest.Gender, src => src.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.DateOfBirth, src => src.MapFrom(src => src.DateOfBirth))
                    .ForMember(dest => dest.Image, src => src.MapFrom(src => src.Image))
                    .ReverseMap();

           CreateMap<Doctor, DoctorResponseDto>()
                .ForMember(dest => dest.Fullname, src => src.MapFrom(src => src.ApplicationUser.FirstName + " " + src.ApplicationUser.LastName))
                .ForMember(dest => dest.specialization, src => src.MapFrom(src => src.Specialization.SpecializaEn))
                .ForMember(dest => dest.Image, src => src.MapFrom(src => src.ApplicationUser.Image))
                .ForMember(dest => dest.DateOfBirth, src => src.MapFrom(src => src.ApplicationUser.DateOfBirth.ToString("yyyy/MM/dd")))
                .ForMember(dest => dest.Email, src => src.MapFrom(src => src.ApplicationUser.Email))
                .ForMember(dest => dest.phone, src => src.MapFrom(src => src.ApplicationUser.PhoneNumber))
                .ForMember(dest => dest.Gender, src => src.MapFrom(src => src.ApplicationUser.Gender))
                .ReverseMap();

  

      


            CreateMap<UpdateDoctorDto,ApplicationUser>()
                .ForMember(dest => dest.FirstName, src => src.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, src => src.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(src => src.phone))
                .ForMember(dest => dest.DateOfBirth, src => src.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Email, src => src.MapFrom(src => src.Email))
                .ForMember(dest => dest.Gender, src => src.MapFrom(src => src.Gender))
                .ReverseMap();


            CreateMap<UpdateDoctorDto,Doctor>()
                .ForMember(dest => dest.Id, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.SpecializationId, src => src.MapFrom(src => src.specializeId))
                .ReverseMap();


            CreateMap<ApplicationUser, PatientDto>()
               .ForMember(dest => dest.Image, src => src.MapFrom(src => src.Image))
               .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FirstName + " " + src.LastName))
               .ForMember(dest => dest.Email, src => src.MapFrom(src => src.Email))
               .ForMember(dest => dest.DateOfBirth, src => src.MapFrom(src => src.DateOfBirth.ToString("MM/dd/yyyy")))
               .ForMember(dest => dest.phone, src => src.MapFrom(src => src.PhoneNumber))
               .ReverseMap();

            CreateMap<Booking, PatientRequestsDto>()
                .ForMember(dest => dest.price, src => src.MapFrom(src => src.Price))
                .ForMember(dest => dest.finalPrice, src => src.MapFrom(src => src.FinalPrice))
                .ForMember(dest => dest.time, src => src.MapFrom(src => src.AppointmentTimes.Time.ToString("HH:mm")))
                .ForMember(dest => dest.DiscoundCode, src => src.MapFrom(src => src.Discount.DiscountCode))
                .ReverseMap();


            CreateMap<Booking, PatientbookingsDto>()
                .ForMember(dest => dest.price, src => src.MapFrom(src => src.Price.ToString("C")))
                .ForMember(dest => dest.finalPrice, src => src.MapFrom(src => src.FinalPrice.ToString("C")))
                .ForMember(dest => dest.discoundCode, src => src.MapFrom(src => src.Discount.DiscountCode))
                .ForMember(dest => dest.docotorName, src => src.MapFrom(src => src.Doctor.ApplicationUser.FirstName+ " " + src.Doctor.ApplicationUser.LastName))
                .ForMember(dest => dest.time, src => src.MapFrom(src => src.AppointmentTimes.Time.ToString("hh:MM")))
                .ForMember(dest => dest.image, src => src.MapFrom(src => src.Doctor.ApplicationUser.Image))
                .ForMember(dest => dest.status, src => src.MapFrom(src => src.RequestStauts.ToString()))
                .ReverseMap();

        }
    }
}
