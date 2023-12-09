namespace VezeetaProject.Core.Dtos.DoctorDtos
{
    public class UpdateDoctorDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public IFormFile Image { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }
        [Required,Phone]
        public string phone { get; set; }
        [Required]
        public int specializeId { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
