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
    public class GradeDTO
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

       
        public string StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }

        //[JsonProperty("ClassSubjectTeacherID")]
        //public int ClassSubjectTeacherID { get; set; }
        //[JsonProperty("SubjectTeacherID")]
        //public int ClassSubjectTeacherSubjectTeacherID { get; set; }
        [JsonProperty("ClassID")]
        public int StudentClassID { get; set; }
        [JsonProperty("ClassLabel")]
        public string StudentClassLabel { get; set; }
        [JsonProperty("ClassYear")]
        public int ?StudentClassYear { get; set; }
        [JsonProperty("SubjectID")]
        public int ClassSubjectTeacherSubjectTeacherSubjectID { get; set; }
        [JsonProperty("SubjectName")]
        public string ClassSubjectTeacherSubjectTeacherSubjectName { get; set; }
        [JsonProperty("TeacherID")]
        public string ClassSubjectTeacherSubjectTeacherTeacherId { get; set; }
        [JsonProperty("TeacherName")]
        public string ClassSubjectTeacherSubjectTeacherTeacherFirstName{ get; set; }
        [JsonProperty("TeacherLastName")]
        public string ClassSubjectTeacherSubjectTeacherTeacherLastName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public virtual Semester Semester { get; set; }
        public int? Year { get; set; }
        
        public int GradeValue { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd.MM.yyyy}")]
        public DateTime? GradeDate { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public GradeType GradeType { get; set; }

        public bool Changed { get; set; }
    }
}