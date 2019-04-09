using Newtonsoft.Json;

using Projekat.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class StudentDTO: UserDTO
    {
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd.MM.yyyy.}")]
        [JsonProperty(Order = 6)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}"
            , ApplyFormatInEditMode = true)]
        [ValidateDateOfBirth]
        public DateTime ?DateOfBirth { get; set; }
        [JsonProperty(Order = 7)]
        public string ImagePath { get; set; }
        //[EmailAddress]
        //public string Email { get; set; }
        [JsonProperty(Order = 8)]
        public virtual ClassDTO Class { get; set; }
        [JsonProperty(Order = 9)]
        public virtual ParentDTO Parent { get; set; }

      
    }
}