using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
   
    public class ClassSubjectTeacher
    {
        public int ID { get; set; }
       
        [Required]
        public virtual Class Class { get; set; }

        [Required]
        public virtual SubjectTeacher SubjectTeacher { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
       
        public ClassSubjectTeacher() {

            Grades = new List<Grade>();
        }
    }
}