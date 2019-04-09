using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Class
    {
        public int ID { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string Label { get; set; }
        [Required]
        [Range(1, 8)]
        public int ?Year { get; set; }
        //public DateTime ?StartOfYear { get; set; }

        public virtual ICollection<Student> Students { get; set; }
       
        public virtual ICollection<ClassSubjectTeacher> AttendedTeacherSubjects { get; set; }

        
        public Class()
        {
            Students = new List<Student>();
            AttendedTeacherSubjects = new List<ClassSubjectTeacher>();
        }
    }
}