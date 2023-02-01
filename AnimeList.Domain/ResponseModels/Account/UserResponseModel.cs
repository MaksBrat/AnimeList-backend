using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.ResponseModels.Account
{
    public class UserResponseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

    }
}
