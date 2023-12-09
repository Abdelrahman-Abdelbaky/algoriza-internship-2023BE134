

using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Serialization;

namespace VezeetaProject.Core.Dtos.AuthenticationDtos
{
    public class RegisterDto
    {
        [Required, MaxLength(150)]
        public string FirstName { get; set; }
        [Required, MaxLength(150)]
        public string LastName { get; set; }
        [Required, MaxLength(150)]
        public string Password { get; set; }
        [EmailAddress, Required, MaxLength(150)]
        public string Email { get; set; }
        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string phone { get; set; }
        public IFormFile? Image { get; set; }

    }
}
