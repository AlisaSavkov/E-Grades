using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class SubjectTeacherDTO
    {
        public int ID { get; set; }

       
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string SubjectYear { get; set; }
        public string TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
       

    }
}