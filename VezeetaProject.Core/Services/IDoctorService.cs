namespace VezeetaProject.Core.Services
{
    public interface IDoctorService
    {
        Task<ResultDto<DoctorResponseDto>> GetDoctorById(int Id);
        Task<List<DoctorResponseDto>> GetAllDoctor (int page ,int pageSize ,String search);
        Task<List<DoctorAppointmentsDto2>> GetAllDoctorWithAppointment(int page, int pageSize, string search);
        Task<ResultDto<Doctor>> AddDoctor (DoctorDto model);
        Task<ResultDto<Doctor>> UpdateDoctor (UpdateDoctorDto model);
        Task<ResultDto<Doctor>> DeleteDoctor (int Id);

      
    }
}
