using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models
{

    public enum Semester
    {
        FIRST, SECOND
    };
    public enum GradeType
    {
        REGULAR, HALFYEAR, FINAL
    }
    public class Grade
    {
        public int ID { get; set; }

        [Required]
        public virtual Student Student { get; set; }
        //[Required]
        //public int? ClassSubjectTeacherId {get; set;}

        [Required]
        public virtual ClassSubjectTeacher ClassSubjectTeacher {get; set;}
        [Required]
        public virtual Semester Semester { get; set; }
        [Range(1, 8)]
        public int? Year { get; set; }
        [Required]
        [Range(1, 5)]
        public int GradeValue { get; set; }
        [Required]
        public DateTime? GradeDate { get; set; }
        [Required]
        public GradeType GradeType { get; set; }
        public bool Changed { get; set; }

        
        public Grade()
        {

        }

    }
}