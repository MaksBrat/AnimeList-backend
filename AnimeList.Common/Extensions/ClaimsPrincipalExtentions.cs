using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
