using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.Enums
{   
    //anime status in user anime list
    public enum AnimeListStatus
    {
        Watched = 1,
        Watching = 2,
        WantToWatch = 3,
        Stalled = 4,
        Dropped = 5
    }
}
