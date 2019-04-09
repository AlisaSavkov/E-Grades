using Projekat.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    public class StudentPrivateDTO:UserPrivateDTO
    {
       
        public string ImagePath { get; set; }
        public virtual ClassDTO Class { get; set; }
    }
}