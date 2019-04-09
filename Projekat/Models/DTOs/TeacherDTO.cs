using Newtonsoft.Json;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class TeacherDTO: UserDTO
    {
        [JsonProperty(Order = 6)]
        //[StringLength (13, ErrorMessage = "The {0} must be  {1} characters long.", MinimumLength = 13)]
        [RegularExpression("^[0-9]{13}$", ErrorMessage = "JMBG must contain 13 digits.")]
        public string JMBG { get; set; }
    }
}