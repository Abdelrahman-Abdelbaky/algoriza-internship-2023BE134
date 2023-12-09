using Microsoft.Extensions.Localization;
using VezeetaProject.Core.Dtos.AuthenticationDtos;
using VezeetaProject.Core.Resources;

namespace VezeetaProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
      private readonly IAuthService _authService;
      private readonly IImageService _imageService;
      private readonly IStringLocalizer<SharedResources> _localizer;
        public AuthenticationController(IAuthService authService, IImageService imageService , IStringLocalizer<SharedResources> localizer)
        {
            _authService = authService;
            _imageService = imageService;
            _localizer = localizer;
        }
     
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.SignInAsync(model);

            if (!result.IsAuthenticated)
                return NotFound(result.Message);

            return Ok(result);
        }
     
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            if (model.Image != null)
            {
                if (!_imageService.CheckTypeOfImage(model.Image))
                    return BadRequest(_localizer[ResourceItem.ImageTypeError].ToString());
            }

            var result = await _authService.RegisterAsync(model,Roles.Patient.ToString());

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

     


    }
}
