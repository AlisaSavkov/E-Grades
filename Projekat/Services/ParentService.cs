using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NLog;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Repository;

namespace Projekat.Services
{
    public class ParentService : IParentService
    {
        private IUnitOfWork db;
        private IUserService userService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ParentService(IUnitOfWork db, IUserService userService)
        {
            this.db = db;
            this.userService = userService;
        }


        public ParentDTO Delete(string id)
        {
            Parent removed = GetById(id);

            if (removed.Children.Count != 0)
            {

                logger.Info("Exception - Can't delete parent with children!");
                throw new Exception("Can't delete parent with children!");
            }
            db.ParentsRepository.Delete(id);
            db.Save();
            ParentDTO removedDto = Mapper.Map<Parent, ParentDTO>(removed);

            return removedDto;
        }
            
    
        //public IEnumerable<ParentDTO> GetAllParents()
        //{
        //    return db.ParentsRepository.Get().ToList().Select(Mapper.Map<Parent, ParentDTO>);
        //}

        public IEnumerable<Parent> GetAllParents()
        {
            return db.ParentsRepository.Get();
        }

        public Parent GetById(string id)
        {
            var user = userService.GetByUserID(id);
            if (user != null)
            {
                IList<string> roles = userService.FindRoles(id);
                if (!roles.Contains("parents"))
                {
                    logger.Info("Exception - user is not a parent.");
                    throw new Exception("User is not a parent.");
                }
                else
                {
                    return db.ParentsRepository.GetByID(id);
                }

            }
            logger.Info("Exception - Parent is not found.");
            throw new Exception("Parent is not found.");

        }
        public ParentDTO GetDtoById(string id)
        {
            Parent found = GetById(id);
            return Mapper.Map<Parent, ParentDTO>(found);
        }

        public ParentDTO Update(string id, ParentDTO dto)
        {
            Parent updated = GetById(id);

           
                if (dto.JMBG != null && dto.JMBG != updated.JMBG && db.ParentsRepository.Get().Where(x => x.JMBG != null && x.JMBG == dto.JMBG).FirstOrDefault() == null)
                {
                    updated.JMBG = dto.JMBG;
                }
                
                if (dto.UserName != null && dto.UserName != updated.UserName && userService.GetByUserName(dto.UserName) == null)
                {
                    updated.UserName = dto.UserName;
                }
                if (dto.Email != null && dto.Email != updated.Email && userService.GetByEmail(dto.Email) == null)
                {
                    updated.Email = dto.Email;
                }
                if (dto.LastName != null)
                {
                    updated.LastName = dto.LastName;
                }

                if (dto.FirstName != null)
                {
                    updated.FirstName = dto.FirstName;
                }

                db.ParentsRepository.Update(updated);
                db.Save();
                
                return Mapper.Map<Parent, ParentDTO>(updated);
           
        }

        public Parent GetByUserName(string userName)
        {
            return db.ParentsRepository.Get().Where(x => x.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
        }

        public Parent GetByJMBG(string jmbg)
        {
            return db.ParentsRepository.Get().Where(x => x.JMBG == jmbg).FirstOrDefault();
        }
    }
}