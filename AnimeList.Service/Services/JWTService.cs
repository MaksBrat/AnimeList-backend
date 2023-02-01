using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.Enum;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.Account;
using AnimeList.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace AnimeList.Services.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public JWTService(IConfiguration configuration, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public string GenerateAccessToken(IEnumerable<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            _ = int.TryParse(_configuration["Jwt:TokenValidityInDays"], out int TokenValidityInDays);

            var tokenOptions = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                authClaims,
                expires: DateTime.Now.AddDays(TokenValidityInDays),
                signingCredentials: signIn);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
                
            return principal;
        }

        public async Task<IBaseResponse<AuthenticatedResponse>> RefreshToken(TokenRequestModel tokenModel)
        {   
            if(tokenModel == null)
            {
                return new BaseResponse<AuthenticatedResponse>
                {
                    Description = "Invalid client request",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };
            }

            string accessToken = tokenModel.AccessToken!;
            string refreshToken = tokenModel.RefreshToken!;

            var principal = GetPrincipalFromExpiredToken(accessToken);

            string userName = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new BaseResponse<AuthenticatedResponse>
                {
                    Description = "Invalid client request",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                };
            }

            var newAccessToken = GenerateAccessToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new BaseResponse<AuthenticatedResponse>
            {
                Data = new AuthenticatedResponse
                {
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken
                },
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }
    }
}
