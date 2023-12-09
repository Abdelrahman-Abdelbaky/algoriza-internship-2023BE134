namespace VezeetaProject.Core.Dtos.DoctorDtos
{
    public class DoctorDto
    {
        [Required]
        public IFormFile Image { get; set; }
        [Required,MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [Required, MaxLength(100),EmailAddress]
        public string Email { get; set; }
        [Required]
        public int phone { get; set; }
        [Required]
        public int specializeId { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

    }
}
