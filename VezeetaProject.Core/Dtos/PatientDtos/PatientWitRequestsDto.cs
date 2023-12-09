namespace VezeetaProject.Core.Dtos.PatientDtos
{
    public class PatientWitRequestsDto
    {

        public PatientWitRequestsDto()
        {
            requests = new List<PatientRequestsDto>();
            Patient = new PatientDto();
        }
        public PatientDto Patient { get; set; }

        public List<PatientRequestsDto> requests { get; set; }
    }
}
