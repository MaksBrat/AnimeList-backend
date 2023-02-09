using AnimeList.Domain.Entity.AnimeNews;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.Entity.Account
{
    public class ApplicationUser : IdentityUser<int>
    {   
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public UserProfile Profile { get; set; }
    }
}
