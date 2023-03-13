using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.RequestModels
{
    public class ProfileRequest
    {   
        public string Name { get; set; }

        [Range(1, 110, ErrorMessage = "Invalid age")]
        public int? Age { get; set; }
    }
}
