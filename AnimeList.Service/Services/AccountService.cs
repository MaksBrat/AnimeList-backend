using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.RequestModels.Account;
using AnimeList.Domain.Response;
using AnimeList.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using AnimeList.Domain.ResponseModels.Account;

namespace AnimeList.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJWTService _jwtService;
        private readonly IProfileService _profileService;
        private readonly IConfiguration _configuration;

        public AccountService(UserManager<ApplicationUser> userManager, IJWTService jwtService, 
            IProfileService profileService, IConfiguration configuration)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _profileService = profileService;
            _configuration = configuration;
        }

        public async Task<IBaseResponse<IdentityUser>> Register(RegisterModel model)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                {
                    return new BaseResponse<IdentityUser>()
                    {
                        Description = "User with this email already exists!",
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                }

                ApplicationUser user = new()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                result = await _userManager.AddToRoleAsync(user, "Admin");

                if (!result.Succeeded)
                {
                    return new BaseResponse<IdentityUser>()
                    {
                        Description = "Failed to create user, please try again.",
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                }

                await _profileService.Create(user);

                return new BaseResponse<IdentityUser>()
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IdentityUser>()
                {
                    Description = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponse<AuthenticatedResponse>> Login(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())                       
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var accessToken = _jwtService.GenerateAccessToken(authClaims);
                    var refreshToken = _jwtService.GenerateRefreshToken();

                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                    await _userManager.UpdateAsync(user);

                    return new BaseResponse<AuthenticatedResponse>()
                    {
                        Data = new AuthenticatedResponse
                        {
                            Token = accessToken,
                            RefreshToken = refreshToken,
                            UserId = user.Id
                        },
                        StatusCode = HttpStatusCode.OK
                    };
                }
                return new BaseResponse<AuthenticatedResponse>()
                {
                    Description = "Failed to login user, please try again.",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            catch (Exception ex)
            {          
                return new BaseResponse<AuthenticatedResponse>()
                {
                    Description = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponse<ApplicationUser>> Logout(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new BaseResponse<ApplicationUser>()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return new BaseResponse<ApplicationUser>()
            {
                StatusCode = HttpStatusCode.OK
            };
        }      
    }
}
