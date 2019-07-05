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
    public class TeacherService : ITeacherService
    {
        private IUnitOfWork db;
        private IUserService userService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TeacherService(IUnitOfWork db, IUserService userService)
        {
            this.db = db;
            this.userService = userService;
            
        }

        public IEnumerable<TeacherDTO> GetAllTeachersDTO()
        {
            return db.TeachersRepository.Get().ToList().Select(Mapper.Map<Teacher, TeacherDTO>);
        }

        public Teacher GetById(string id)
        {

            var user = userService.GetByUserID(id);
            if (user != null)
            {
                IList<string> roles = userService.FindRoles(id);
                if (!roles.Contains("teachers"))
                {
                    logger.Info("Exception - user is not a teacher.");
                    throw new Exception("User is not a teacher.");
                }
                else
                {
                    return db.TeachersRepository.GetByID(id);
                }

            }
            logger.Info("Exception - teacher is not found.");
            throw new Exception("Teacher is not found.");

        }

        public TeacherDTO GetDtoById(string id)
        {
            Teacher found = GetById(id);
            return Mapper.Map<Teacher, TeacherDTO>(found);
        }

        public IEnumerable<TeacherDTO> GetTeachersBySubject(int subjectId)
        {
            Subject subject = db.SubjectsRepository.GetByID(subjectId);
            if(subject == null)
            {
                logger.Info("Subject with id " + subjectId + " doesn't exist.");
                throw new Exception("Subject with id " + subjectId + " doesn't exist.");
            }

            IEnumerable<SubjectTeacher> taughtSubjects = db.SubjectTeachersRepository.GetBySubjectId(subjectId);
            List<Teacher> teachers = new List<Teacher>();
            foreach (var item in taughtSubjects)
            {
                teachers.Add(item.Teacher);
            }
            return teachers.ToList().Select(Mapper.Map<Teacher, TeacherDTO>);
        }

       

        public TeacherDTO Update(string id, TeacherDTO dto)
        {
            Teacher updated = GetById(id);

               if(dto.UserName != null && dto.UserName != updated.UserName && userService.GetByUserName(dto.UserName) != null)
                {
                    logger.Info("User with username " + dto.UserName + " already exists!");
                    throw new Exception("User with username " + dto.UserName + " already exists!");
                }

                if (dto.JMBG != null && dto.JMBG != updated.JMBG && db.TeachersRepository.Get().Where(x => x.JMBG == dto.JMBG).FirstOrDefault() != null)
                {
                    logger.Info("Teacher with JMBG " + dto.JMBG + " already exists!");
                    throw new Exception("Teacher with JMBG " + dto.JMBG + " already exists!");
                }

                if (dto.UserName != null && dto.UserName != updated.UserName)
                {
                    updated.UserName = dto.UserName;
                }

                if (dto.Email != null)
                {
                    updated.Email = dto.Email;
                }
                if (dto.JMBG != null)
                {
                    updated.JMBG = dto.JMBG;
                }

                if (dto.LastName != null)
                {
                    updated.LastName = dto.LastName;
                }
                
                if (dto.FirstName != null)
                {
                    updated.FirstName = dto.FirstName;
                }

                db.TeachersRepository.Update(updated);
                db.Save();
               
                return Mapper.Map<Teacher, TeacherDTO>(updated);
            
        }
        
        public TeacherDTO Delete(string id)
        {
            Teacher removed = GetById(id);

            IEnumerable<SubjectTeacher> sts = db.SubjectTeachersRepository.GetByTeacherId(id);
            if (sts.Count() > 0)
            {
                logger.Info("Can't delete teacher that teaches subjects!");
                throw new Exception("Can't delete teacher that teaches subjects!");
            }


            db.TeachersRepository.Delete(id);
            db.Save();
            TeacherDTO removedDto = Mapper.Map<Teacher, TeacherDTO>(removed);
            return removedDto;

        }

        public IEnumerable<Teacher> GetAllTeachers()
        {
            return db.TeachersRepository.Get();
        }


    }
}