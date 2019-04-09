using Microsoft.AspNet.Identity;
using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Repositories
{
    public interface IAuthRepository : IDisposable
    {
        Task<IdentityResult> RegisterTeacherUser(Teacher teacher, string password);
        Task<IdentityResult> RegisterAdminUser(Admin admin, string password);
        Task<IdentityResult> RegisterStudentUser(Student student, string password);
        Task<IdentityResult> RegisterParentUser(Parent parent, string password);


        Task<ApplicationUser> FindUser(string userName, string password);
        //ApplicationUser FindById(string id);
        //Task<ApplicationUser> FindById(string id);
        Task<ApplicationUser> FindByUserName(string userName);
        //ApplicationUser FindByUName(string userName);

        Task<ApplicationUser> FindByEmail(string email);
        Task<IList<string>> FindRoles(string userId);
        //IList<string> FindRoles1(string userId);
    }
}
