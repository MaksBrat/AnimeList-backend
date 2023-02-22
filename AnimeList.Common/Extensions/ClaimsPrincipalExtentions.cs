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
        public static int GetUserId(this ClaimsPrincipal userClaims)
        {
            try
            {
                string claim = userClaims.Claims.FirstOrDefault(w => w.Type == ClaimTypes.NameIdentifier)?.Value;
                return Int32.Parse(claim);
            }
            catch
            {
                return 0;
            }
        }
    }
}
