using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.RequestModels
{
    public class ProfileRequestModel
    {   
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
