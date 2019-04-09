using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Projekat.Models;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class ClassSubjectTeacherDTO
    {
        public int ID { get; set; }
      
        public int ClassID { get; set; }
        public string ClassLabel { get; set; }
        public int? ClassYear { get; set; }
        //public virtual SubjectTeacherDTO SubjectTeacher { get; set; }
        public string SubjectTeacherID { get; set; }
        [JsonProperty("SubjectID")]
        public int SubjectTeacherSubjectID { get; set; }
        [JsonProperty("SubjectName")]
        public string SubjectTeacherSubjectName { get; set; }
        [JsonProperty("TeacherId")]
        public string SubjectTeacherTeacherId { get; set; }
        [JsonProperty("TeacherFirstName")]
        public string SubjectTeacherTeacherFirstName { get; set; }
        [JsonProperty("TeacherLastName")]
        public string SubjectTeacherTeacherLastName { get; set; }

        //[JsonConverter(typeof(StringEnumConverter))]
        //public virtual Semester ?Semester { get; set; }
    }
}