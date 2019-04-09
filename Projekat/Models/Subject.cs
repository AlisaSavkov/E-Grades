using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Subject
    {

        public int ID { get; set; }
        [Required]
        public string Name { get; set;}
        public int ?LessonNumber { get; set; }
        [Required]
        [Range(1, 8)]
        public int ?Year { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<SubjectTeacher> SubjectTeachers { get; set; }

       
        public Subject()
        {
            SubjectTeachers = new HashSet<SubjectTeacher>();
        }
    }
}