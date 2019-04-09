using Microsoft.AspNet.Identity;
using Projekat.Models.DTOs;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface IUserRegistrationService
    {
       
        Task<IdentityResult> RegisterAdminUser(AdminRegistrationDTO user);
        Task<IdentityResult> RegisterTeacherUser(TeacherRegistrationDTO user);
        Task<IdentityResult> RegisterParentUser(ParentRegistrationDTO dto);
        //Task<IdentityResult> RegisterStudentUser(UserRegistrationDTO user);
        Task<IdentityResult> RegisterStudentParent(StudentParentRegistrationDTO studentParent);
        Task<IdentityResult> RegisterStudent(StudentRegistrationDTO dto);
        Task<IdentityResult> RegisterSPAsync(SPRegistrationDTO dto);
    }
}
