
using VezeetaProject.Core.Dtos.Booking;

namespace VezeetaProject.Core.Services
{
    public interface IBookingService
    {
        Task<ResultDto<AddBookingDto>> Booking(AddBookingDto model, string id);
        Task<ResultDto<ResponeBookingDto>> GetAllBooking(ResponeBookingDto model, string id);
        Task<ResultDto<Booking>> CancelBooking(int id);
        Task<ResultDto<Booking>> ConfiromCheckUp(int Bookingid, string DocctorId);
        Task<List<PatientbookingsDto>> GetAll(string PatientId);
        Task<List<PatientWithAppointmentDto>> GetAllBookingAppointmentForDoctor(int Page, int PageSize, Days Search, string DoctorId);
    }
}
