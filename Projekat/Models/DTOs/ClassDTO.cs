using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class ClassDTO
    {
        [JsonProperty(PropertyName = "Id")]
        public int ID { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "The {0} must contain between {2} and {1} characters.", MinimumLength = 1)]
        public string Label { get; set; }
        [Required]
        [Range(1, 8,ErrorMessage = "Year must be a number between {0} and {1}.")]
        public int ?Year { get; set; }

        
    }
}