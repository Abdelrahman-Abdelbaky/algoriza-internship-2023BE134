namespace VezeetaProject.Core.Dtos.AuthenticationDtos
{
    public class SignInDto
    {
        [EmailAddress, Required, MaxLength(150)]
        public string Email { get; set; }
        [Required, MaxLength(150)]
        public string password { get; set; }
    }
}
