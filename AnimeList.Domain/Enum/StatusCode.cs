using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.Enum
{
    public enum StatusCode
    {
        UserNotFound = 0,
        AnimeNotFound = 1,
        MangaNotFound =2,

        OK = 200,
        InternalServerError = 500
    }
}
