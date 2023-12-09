using VezeetaProject.Core.Dtos.AppointmentDtos;

namespace VezeetaProject.Core.Services
{
    public interface IAppointmentServices
    {
        Task<ResultDto<DoctorAppointment>> AddAppointmentAsync (AddAppointmentDto model, string userId);
        Task<ResultDto<UpdateTimeDto>>  UpdateAppointmentAsync (ModfiyAppointmentDto model, string userId);
        Task<ResultDto<UpdateTimeDto>> DeleteAppointmentAsync(int id, string userId);
    }
}
