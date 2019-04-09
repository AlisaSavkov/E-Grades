using Newtonsoft.Json;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class StudentPDTO
    {
        [JsonProperty(Order = 1)]
        public string ID { get; set; }
        [JsonProperty(Order = 2)]
        public string FirstName { get; set; }
        [JsonProperty(Order = 3)]
        public string LastName { get; set; }
        [JsonProperty(Order = 4)]
        public DateTime? DateOfBirth { get; set; }
        [JsonProperty(Order = 5)]
        
        public string ImagePath { get; set; }
        //[JsonProperty(Order = 6)]
        //public virtual ClassDTO Class { get; set; }
        [JsonProperty(Order = 6)]
        public string ParentID { get; set; }
        [JsonProperty(Order = 7)]
        public string ParentFirstName { get; set; }
        [JsonProperty(Order = 9)]
        public string ParentLastName { get; set; }
        [JsonProperty(Order = 10)]
        public string ClassYear { get; set; }
        [JsonProperty(Order = 11)]
        public string ClassLabel { get; set; }
    }
}