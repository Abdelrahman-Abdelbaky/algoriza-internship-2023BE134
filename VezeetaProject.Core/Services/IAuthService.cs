using VezeetaProject.Core.Dtos.AuthenticationDtos;

namespace VezeetaProject.Core.Services
{
    public interface IAuthService
    {
         Task<AuthDto> RegisterAsync(RegisterDto model, string role);
         Task<AuthDto> SignInAsync(SignInDto model);
    
    }
}
