using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class SubjectTeacher
    {
        public int ID { get; set; }

        [Required]
        public virtual Subject Subject { get; set; }
        [Required]
        public virtual Teacher Teacher { get; set; }

        [JsonIgnore]
        public virtual ICollection<ClassSubjectTeacher> TaughtSubjectClasses { get; set; }

        

        public SubjectTeacher()
        {
            TaughtSubjectClasses = new List<ClassSubjectTeacher>();
            

        }
    }
}