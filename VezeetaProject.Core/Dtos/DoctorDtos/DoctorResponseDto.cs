namespace VezeetaProject.Core.Dtos.DoctorDtos
{
    public class DoctorResponseDto
    {
     
        [Required, MaxLength(150)]
        public string Fullname { get; set; }
     
        public string Email { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public string specialization { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public Byte[] Image { get; set; }
    }
}
