using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Projekat.Models;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface IUserService
    {
        //IEnumerable<ApplicationUser> GetApplicationUsers();
        IEnumerable<UserDTO> GetApplicationUsers();
        //ApplicationUser GetByEmail(string Email);
        UserDTO GetByEmail(string Email);
        Task<IdentityUser> FindByEmail(string email);
        ApplicationUser GetByUserName(string username);
        ApplicationUser GetByUserID(string id);
        IList<string> FindRoles(string userId);

    }
}
