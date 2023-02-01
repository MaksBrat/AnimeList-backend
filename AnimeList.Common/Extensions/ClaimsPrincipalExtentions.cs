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
        public static string GetUserId(this ClaimsPrincipal userClaims)
        {
            try
            {
                string claim = userClaims.Claims.FirstOrDefault(w => w.Type == ClaimTypes.NameIdentifier)?.Value;
                return claim;
            }
            catch
            {
                throw new SystemException("User is not found");
            }
        }

        public static string GetUserRole(this ClaimsPrincipal userClaims)
        {
            try
            {
                var claim = userClaims.Claims.FirstOrDefault(w => w.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(claim))
                    throw new SystemException("User is not found");

                return claim;
            }
            catch
            {
                throw new SystemException("User is not found");
            }
        }
    }
}
