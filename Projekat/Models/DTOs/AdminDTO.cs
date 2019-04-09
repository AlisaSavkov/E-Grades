using Newtonsoft.Json;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class AdminDTO: UserDTO
    {
        [JsonProperty(Order = 6)]
        public string ShortName { get; set; }
    }
}