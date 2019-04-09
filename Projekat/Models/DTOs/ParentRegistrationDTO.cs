using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class ParentRegistrationDTO: UserRegistrationDTO
    {
        [RegularExpression("^[0-9]{13}$", ErrorMessage = "JMBG must contain 13 digits.")]
        public string JMBG { get; set; }
    }
}