﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.ResponseModels.Profile
{
    public class UserProfileResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public byte[] Avatar { get; set; }
        public DateTime RegistratedAt { get; set; }
    }
}