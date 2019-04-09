using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class StudentTDTO
    {
        [JsonProperty(Order = 6)]
        public string ID { get; set; }
        [JsonProperty(Order = 7)]
        public string FirstName { get; set; }
        [JsonProperty(Order = 8)]
        public string LastName { get; set; }
        [JsonProperty(Order = 9)]
        public DateTime? DateOfBirth { get; set; }
        [JsonProperty(Order = 10)]
        public string ImagePath { get; set; }
        [JsonProperty(Order = 11)]
        public virtual ClassDTO Class { get; set; }
        [JsonProperty(Order = 12)]
        public virtual ParentTDTO Parent { get; set; }

    }
}