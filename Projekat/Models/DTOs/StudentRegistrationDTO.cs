using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class StudentRegistrationDTO: UserRegistrationDTO
    {

        //public virtual ParentRegistrationDTO Parent { get; set; }
        public DateTime ?DateOfBirth { get; set; }
        public int classID{ get; set; }
    }
}