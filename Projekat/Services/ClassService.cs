using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NLog;
using Projekat.Models.DTOs;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Repository;

namespace Projekat.Services
{
    public class ClassService : IClassService
    {

        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ClassService(IUnitOfWork db)
        {
            this.db = db;
        }

        
        public IEnumerable<ClassDTO> GetAllClasses()
        {
            IEnumerable<ClassDTO> cl = db.ClassesRepository.Get().ToList().Select(Mapper.Map<Class, ClassDTO>);
            IEnumerable<ClassDTO> result = cl.OrderBy(c => c.Year);
            return result;
            //return db.ClassesRepository.Get().ToList().Select(Mapper.Map<Class, ClassDTO>);
        }

        public ClassDTO GetDtoById(int id)
        {
            Class found = db.ClassesRepository.GetByID(id);
            return Mapper.Map<ClassDTO>(found);
        }

        public Class GetById(int id)
        {
           
            return db.ClassesRepository.GetByID(id);
        }
        public ClassDTO Create(ClassDTO dto)
        {
            if (dto == null)
            {
                return null;
            }

            Class found = db.ClassesRepository.GetByYearLabel(dto.Year, dto.Label);

            if (found == null)
            {
                Class newClass = new Class();
                newClass.Label = dto.Label;
                newClass.Year = dto.Year;

                db.ClassesRepository.Insert(newClass);

                db.Save();
                ClassDTO newDto = Mapper.Map<Class, ClassDTO>(newClass);
                return newDto;
            }
            logger.Info("Exception - Class with inserted label and year already exists!");
            throw new Exception("Class with inserted label and year already exists!");
        }

        public ClassDTO Delete(int id)
        {
            Class removed = db.ClassesRepository.GetByID(id);
            if (removed != null)
            {
               
                    if(removed.Students.Count != 0)
                    {
                    logger.Info("Exception - Can't delete class with students!");
                    throw new Exception("Can't delete class with students!");
                    }
                    db.ClassesRepository.Delete(id);
                    db.Save();
                }
                
            ClassDTO removedDto = Mapper.Map<Class, ClassDTO>(removed);
            return removedDto;
 
        }

        public ClassDTO Update(int id, ClassDTO dto)
        {
            Class found = db.ClassesRepository.GetByID(id);

            if (found != null)
                
            {
                
                if (db.ClassesRepository.GetByYearLabel(dto.Year, dto.Label) == null || (found.ID== dto.ID && found.Year == dto.Year&& found.Label == dto.Label))
                {
                    found.Year = dto.Year;
                    found.Label = dto.Label;
                    
                    db.ClassesRepository.Update(found);
                    db.Save();
                    return Mapper.Map<Class, ClassDTO>(found);
                }
                
                logger.Info("Exception - Class with inserted label and year already exists.");
                throw new Exception("Class with inserted label and year already exists!");
            }
            return null;

        }

        //get classes by teacher
        public IEnumerable<ClassDTO> GetAllClassesByTeacher(string teaherId)
        {
            IEnumerable<ClassSubjectTeacher> csts = db.ClassSubjectTeachersRepository.GetByTeacher(teaherId);

            IList<Class> classes = new List<Class>();
            foreach(var item in csts)
            {
                if (!classes.Contains(item.Class))
                {
                    classes.Add(item.Class);
                }
            }
            
            return classes.ToList().Select(Mapper.Map<Class, ClassDTO>);
        }

        public IEnumerable<ClassDTO> GetAllClassesByTeacherSubject(string teacherId, int subjectId)
        {
            IEnumerable<ClassSubjectTeacher> csts = db.ClassSubjectTeachersRepository.GetByTeacherSubject(teacherId, subjectId);

            IList<Class> classes = new List<Class>();
            foreach (var item in csts)
            {
                if (!classes.Contains(item.Class))
                {
                    classes.Add(item.Class);
                }
            }

            return classes.ToList().Select(Mapper.Map<Class, ClassDTO>);
        }
    }
}