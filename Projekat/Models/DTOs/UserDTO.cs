using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class UserDTO
    {

        //[Display(Name = "User name")]
        [JsonProperty(Order = 1)]
        public string ID { get; set; }
        [JsonProperty(Order = 2)]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string UserName { get; set; }
        [JsonProperty(Order = 3)]
        // [RegularExpression("[A-Z/a-z]$", ErrorMessage = "JMBG must contain 13 digits.")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }
        [JsonProperty(Order = 4)]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }
        [JsonProperty(Order = 5)]
        [EmailAddress]
        public string Email { get; set; }

    }
}