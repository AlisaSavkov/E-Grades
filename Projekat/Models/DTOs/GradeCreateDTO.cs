using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Projekat.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class GradeCreateDTO
    {
        
        [Required(ErrorMessage = "Grade value must be provided.")]
        [Range(1, 5, ErrorMessage = "Grade value must be between 1 and 5.")]
        public int GradeValue { get; set; }
       
    }
}