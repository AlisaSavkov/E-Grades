using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Projekat.Models
{
    public class Student : ApplicationUser
    {

        public DateTime ?DateOfBirth { get; set; }
        public string ImagePath { get; set; }
 
        public virtual Parent Parent { get; set; }
        public virtual Class Class { get; set; }

        [JsonIgnore]
        public virtual ICollection<Grade> Grades { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public Student()
        {
            Grades = new List<Grade>();

        }

    }
}