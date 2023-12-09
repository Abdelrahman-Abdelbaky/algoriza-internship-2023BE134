using Microsoft.Extensions.Options;
using VezeetaProject.Core.Dtos.AuthenticationDtos;
using VezeetaProject.Core.Resources;

namespace VezeetaProject.Services
{
    /// <summary>
    ///  service use for authentication  
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IImageService _imageService;
        private readonly JWT _JWT;
        
        public AuthService(UserManager<ApplicationUser> userManager,IOptions<JWT> JWT, IStringLocalizer<SharedResources> Localizer , IImageService  ImageService)
        {
            _userManager = userManager;
            _JWT = JWT.Value;
            _localizer = Localizer;
            _imageService = ImageService;
            
        }

        /// <summary>
        /// take data from the user and valided it to make new account
        /// </summary>
        /// <param name="model"></param>
        /// <param name="role"></param>
        /// <param name="SpecializationId"></param>
        /// <returns>AuthDto</returns>
        public async Task<AuthDto> RegisterAsync(RegisterDto model, string role )
        {

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthDto() { Message = _localizer[ResourceItem.EmailNotFound] };

            var applicationUser = new ApplicationUser()
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber =model.phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Image = (model.Image is not null)? await _imageService.EncodeImageAsync(model.Image):null,
                TimeStamp = DateTime.Now
            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthDto { Message = errors };
            }

            await _userManager.AddToRoleAsync(applicationUser, role);

            var jwtSecurityToken = await CreateJwtToken(applicationUser);

            return new AuthDto
            {
                Email = applicationUser.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { role },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = applicationUser.UserName,
                userId = applicationUser.Id,
               
            };
        }

        /// <summary>
        /// take data from the user and valided it to signin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AuthDto> SignInAsync(SignInDto model)
        {
            var _authDto = new AuthDto();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.password))
            {
                _authDto.Message = _localizer[ResourceItem.PasswordOrEmailIncorrect];
                return _authDto;
            }

            var Token = await CreateJwtToken(user);
            var role = await _userManager.GetRolesAsync(user);
            
            _authDto.Email = user.Email;
            _authDto.Message = _localizer[ResourceItem.Welcome];
            _authDto.IsAuthenticated = true;
            _authDto.ExpiresOn= Token.ValidTo;
            _authDto.Roles = role.ToList();
            _authDto.Token = new JwtSecurityTokenHandler().WriteToken(Token);
            
            return _authDto;
        }


        /// <summary>
        /// create a token to use in authorization
        /// </summary>
        /// <param name="user"></param>
        /// <returns> JwtSecurityToken </returns>
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            #region payLoad
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));
           
           var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
           .Union(userClaims)
           .Union(roleClaims);
            #endregion

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWT.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
              issuer: _JWT.Issuer,
              audience: _JWT.Audience,
              claims: claims,
              expires: DateTime.Now.AddDays(_JWT.DurationInDays),
              signingCredentials: signingCredentials);

            return jwtSecurityToken;

        }
        }
}