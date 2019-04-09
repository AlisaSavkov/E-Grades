using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NLog;
using Projekat.Models;
using Projekat.Repository;

namespace Projekat.Services
{
    public class AdminService : IAdminService
    {
        private IUnitOfWork db;
        private IUserService userService;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        public AdminService(IUnitOfWork db, IUserService userService)
        {
            this.db = db;
            this.userService = userService;
        }

        public AdminDTO Delete(string id)
        {
            Admin removed = GetById(id);

            db.AdminsRepository.Delete(id);
            db.Save();
            AdminDTO removedDto = Mapper.Map<Admin, AdminDTO>(removed);
            return removedDto;
        }


        public IEnumerable<AdminDTO> GetAll()
        {
            return db.AdminsRepository.Get().ToList().Select(Mapper.Map<Admin, AdminDTO>);
        }

        public Admin GetById(string id)
        {
            var user = userService.GetByUserID(id);
            if (user == null)
            {
                logger.Info("Exception - user is not found.");
                throw new Exception("User is not found.");
            }
            IList<string> roles = userService.FindRoles(id);
            if (!roles.Contains("admins"))
            {
                logger.Info("Exception - user is not an admin");
                throw new Exception("User is not an admin.");
            }
            return db.AdminsRepository.GetByID(id);
           
        }

        public AdminDTO GetByUserName(string userName)
        {
            try
            {
                Admin found = db.AdminsRepository.Get().Where(x => x.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
                if (found == null)
                {
                    return null;
                }
                return Mapper.Map<Admin, AdminDTO>(found);
            }
            catch (Exception)
            {
                logger.Info("Exception - user with id " + userName + " is not an admin.");
                throw new Exception("User is not an admin");
            }

        }

        public AdminDTO GetDtoById(string id)
        {
            Admin found = GetById(id);

            return Mapper.Map<Admin, AdminDTO>(found);
        }

        public AdminDTO Update(string id, AdminDTO dto)
        {
            Admin updated = GetById(id);

            //ako je username razlicit od postojeceg i ako ne postoji user sa tim usernamemo menjamo username
            if (dto.UserName != null && dto.UserName != updated.UserName && userService.GetByUserName(dto.UserName) == null)
            {
                updated.UserName = dto.UserName;
            }
            if (dto.Email != null && dto.Email != updated.Email && userService.GetByEmail(dto.Email) == null)
            {
                updated.Email = dto.Email;
            }


            if (dto.FirstName != null)
            {
                updated.FirstName = dto.FirstName;
            }
            if (dto.LastName != null)
            {
                updated.LastName = dto.LastName;
            }

            if (dto.ShortName != null)
            {
                updated.ShortName = dto.ShortName;
            }

            db.AdminsRepository.Update(updated);
            db.Save();
            return Mapper.Map<Admin, AdminDTO>(updated);

        }


    }
}