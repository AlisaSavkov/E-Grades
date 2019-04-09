using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class ClassSubjectTeacherUpdateDTO
    {
        public int ID { get; set; }
       
        public int ClassId { get; set; }
        
        public int SubjectId { get; set; }

        public string TeacherId { get; set; }
    }
}