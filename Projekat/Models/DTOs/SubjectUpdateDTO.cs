using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.ModelsIzmena.DTOs
{
    public class SubjectUpdateDTO
    {
        public int ID { get; set; }
        
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Subject name must be between 3 and 30 character in length.")]
        public string Name { get; set; }
        [Range(1, 10, ErrorMessage = "Lesson number must be a number between 1 and 10 .")]
        public int? LessonNumber { get; set; }
        //[Required(ErrorMessage = "Year must be provided")]
        
        [Range(1, 8, ErrorMessage = "Year must be a number between 1 and 8 .")]
        //[RegularExpression("^[0-8]{1}$", ErrorMessage = "Year must be a number between 1 and 8.")]
        public int? Year { get; set; }
    }
}