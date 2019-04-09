using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Projekat.Models
{
    public class Teacher: ApplicationUser
    {
        [RegularExpression("^[0-9]{13}$")]
        public string JMBG { get; set; }


        [JsonIgnore]
        public virtual ICollection<SubjectTeacher> TaughtSubjects { get; set; }

        public Teacher()
        {
            TaughtSubjects = new HashSet<SubjectTeacher>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}