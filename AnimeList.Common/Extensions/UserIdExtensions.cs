using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Common.Extentions
{
    public static class UserIdExtensions
    {
        private static int id = 0;
        public static int GetId => ++id; 
    }
}
