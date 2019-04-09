using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class StudentParentRegistrationDTO
    {
        [Required]
        public StudentRegistrationDTO Student { get; set; }
        [Required]
        public ParentRegistrationDTO Parent { get; set; }
    }
}