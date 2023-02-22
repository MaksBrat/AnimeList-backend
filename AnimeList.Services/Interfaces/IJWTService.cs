using AnimeList.Domain.RequestModels;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AnimeList.Services.Interfaces
{
    public interface IJWTService
    {
        public string GenerateAccessToken(IEnumerable<Claim> authClaims);
        public string GenerateRefreshToken();
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
        public Task<IBaseResponse<AuthenticatedResponse>> RefreshToken(TokenRequestModel tokenModel);
    }
}
