namespace VezeetaProject.Core.Services
{
    public interface IpatientService
    {
        Task<ResultDto<List<PatientDto>>> GeTAllPatient(int Page , int PageSize, string Search);
        Task<ResultDto<PatientWitRequestsDto>> GeTBYId(string Id);
       
    }
}
