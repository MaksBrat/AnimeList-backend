using System.Security.Claims;

namespace AnimeList.Common.Extentions
{
    public static class ClaimsPrincipalExtentions
    {
        public static int? GetUserId(this ClaimsPrincipal userClaims)
        {
            string claimValue = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(claimValue, out int userId))
            {
                return userId;
            }
            else
            {
                return null;
            }
        }
    }
}
