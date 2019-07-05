using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Repository;

namespace Projekat.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork db;
        private UserManager<ApplicationUser> _userManager;

        public UserService(IUnitOfWork db, DbContext context)
        {
            this.db = db;
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

      

        IEnumerable<UserDTO> IUserService.GetApplicationUsers()
        {
            return db.UsersRepository.Get().ToList().Select(Mapper.Map<ApplicationUser, UserDTO>);
        }

        UserDTO IUserService.GetByEmail(string email)
        {
            ApplicationUser found = db.UsersRepository.Get().Where(x => x.Email == email).FirstOrDefault();
            return Mapper.Map<UserDTO>(found);
        }

        ApplicationUser GetByUserName(string username)
        {
            return db.UsersRepository.Get().Where(x => x.UserName == username).FirstOrDefault();
        }

        public async Task<IdentityUser> FindByEmail(string email)
        {
               return await db.AuthRepository.FindByEmail(email);

        }

       

        ApplicationUser IUserService.GetByUserName(string username)
        {
            return db.UsersRepository.Get().Where(x => x.UserName == username).FirstOrDefault();
            
        }

        public ApplicationUser GetByUserID(string id)
        {
            return db.UsersRepository.GetByID(id);
        }

        public IList<string> FindRoles(string userId)
        {
            return _userManager.GetRoles(userId);
        }


    }
}