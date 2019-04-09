using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class AdminRegistrationDTO: UserRegistrationDTO
    {

        public string ShortName { get; set; }

    }
}