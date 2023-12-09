namespace VezeetaProject.Core.Dtos.PatientDtos
{
    public class PatientDto
    {
        public Byte[]? Image { get; set; }
        
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
 
        [Required,Phone]
        public string phone { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string DateOfBirth { get; set; }
    }
}
