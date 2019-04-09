using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class SubjectDTO
    {
        [JsonProperty(Order = 1)]
        public int ID { get; set; }
        [JsonProperty(Order = 2)]
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Subject name must be between 2 and 20 character in length.")]
        public string Name { get; set; }
        [JsonProperty(Order =3)]
        [Range(1, 10, ErrorMessage = "Fond casova must be a number between 1 and 10 .")]
        public int ?LessonNumber { get; set; }
        //[Required(ErrorMessage = "Year must be provided")]
        [JsonProperty(Order = 4)]
        [Required]
        [Range(1,8, ErrorMessage = "Year must be a number between 1 and 8 .")]
        //[RegularExpression("^[0-8]{1}$", ErrorMessage = "Year must be a number between 1 and 8.")]
        public int ?Year { get; set; }


    }
}