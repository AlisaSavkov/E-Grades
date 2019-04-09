using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NLog;
using Projekat.ModelsIzmena.DTOs;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Repository;

namespace Projekat.Services
{
    public class SubjectService : ISubjectService
    {
        private IUnitOfWork db;
        private ITeacherService teacherService;
        private IStudentService studentService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SubjectService(IUnitOfWork db, ITeacherService teacherService, IStudentService studentService)
        {
            this.db = db;
            this.teacherService = teacherService;
            this.studentService = studentService;
        }

        public IEnumerable<SubjectDTO> GetAllSubjects()
        {
            return db.SubjectsRepository.Get().ToList().Select(Mapper.Map<Subject, SubjectDTO>);
        }

        //proveriti
        public IEnumerable<SubjectDTO> GetSubjectsByTeacher(string teacherId)
        {
            Teacher teacher = teacherService.GetById(teacherId);
            if (teacher == null)
            {
                return null;
            }
            IEnumerable<SubjectTeacher> taughtSubjects = db.SubjectTeachersRepository.GetByTeacherId(teacherId);
            
            List<Subject> subjects = new List<Subject>();
            foreach (var item in taughtSubjects)
            {
                if(!subjects.Contains(item.Subject))
                subjects.Add(item.Subject);
            }
            return subjects.ToList().Select(Mapper.Map<Subject, SubjectDTO>);
        }

       
        public IEnumerable<SubjectDTO> GetSubjectsByStudent(string studentId)
        {
            Student student = studentService.GetById(studentId);
            
            if(student.Class != null)
            {
                IEnumerable<ClassSubjectTeacher> csts = db.ClassSubjectTeachersRepository.GetByClass(student.Class.ID);

                List<SubjectTeacher> sts = new List<SubjectTeacher>();
                foreach (var item in csts)
                {
                    sts.Add(item.SubjectTeacher);
                }

                List<Subject> subjects = new List<Subject>();

                foreach (var item in sts)
                {
                    subjects.Add(item.Subject);
                }
                return subjects.ToList().Select(Mapper.Map<Subject, SubjectDTO>);
            }
            throw new Exception("Student doesn't attend any subject yet!");
        }

        public SubjectDTO GetDtoById(int id)
        {
            
            return Mapper.Map<SubjectDTO>(GetById(id));

        }

        public Subject GetById(int id)
        {
            Subject subject = db.SubjectsRepository.GetByID(id);
            if(subject == null)
            {
                throw new Exception("Subject with id " + id + " not found.");
            }
            return subject;
        }

        public SubjectDTO Create(SubjectDTO dto)
        {
            if (dto == null)
            {
                return null;
            }
            
            if(db.SubjectsRepository.GetByClassAndYear(dto.Year, dto.Name) == null)
            {
                Subject newSubject = new Subject();
                newSubject.Name = dto.Name;
                newSubject.LessonNumber = dto.LessonNumber;
                newSubject.Year = dto.Year;
                db.SubjectsRepository.Insert(newSubject);
                db.Save();
                SubjectDTO newDto = Mapper.Map<Subject, SubjectDTO>(newSubject);
                return newDto;
            }

            else
            {
                throw new Exception("Inserted subject already exists!");
            }
        }
        //proveriti
        public SubjectDTO Delete(int id)
        {
            Subject subject = GetById(id);
            //if (subject != null)
            //{

                //IEnumerable<ClassSubjectTeacher> csts = db.ClassSubjectTeachersRepository.GetBySubject(id);
                //if(csts.Count() > 0)
                //{
                //    throw new Exception("Can't delete a subject which is taught in classes by teacher!");
                //}
                IEnumerable<SubjectTeacher> sts = db.SubjectTeachersRepository.GetBySubjectId(id);
                if(sts.Count() > 0)
                {

                    throw new Exception("Can't delete a subject which is taught by at least one teacher.");
                    //foreach (var st in sts)
                    //{
                    //    db.SubjectTeachersRepository.Delete(st);
                    //}
                }
                 db.SubjectsRepository.Delete(id);
                 db.Save();
                 SubjectDTO removedDto = Mapper.Map<Subject, SubjectDTO>(subject);
                 return removedDto;
                 
            //}
            //return null;
        }

        public SubjectDTO Update(int id, SubjectUpdateDTO dto)
        {
            Subject updatedSubject = GetById(id);
            
            if(dto.Name!= null && dto.Name == updatedSubject.Name && dto.Year != null && dto.Year == updatedSubject.Year)
            {
                return Mapper.Map<Subject, SubjectDTO>(updatedSubject);
            }

            if(dto.Name != null)
            {
                updatedSubject.Name = dto.Name;
            }
            if(dto.Year != null)
            {
                updatedSubject.Year = dto.Year;
            }
            if (db.SubjectsRepository.GetByClassAndYear(updatedSubject.Year, updatedSubject.Name) != null)
            {
                logger.Info("Exception - Subject with that name and year already exists exists.");
                throw new Exception("The subject with that name and year already exists exists!");
            }
            if (dto.LessonNumber != null)
            {
                updatedSubject.LessonNumber = dto.LessonNumber;
            }

            db.SubjectsRepository.Update(updatedSubject);
            db.Save();

            return Mapper.Map<Subject, SubjectDTO>(updatedSubject); 

        }

        public IEnumerable<SubjectDTO> GetSubjectsNoByTeacher(string teacherId)
        {
            Teacher teacher = teacherService.GetById(teacherId);
            if (teacher == null)
            {
                return null;
            }
            IEnumerable<SubjectDTO> allSubjects = GetAllSubjects();

            IEnumerable<SubjectDTO> tauGhtSubjects = GetSubjectsByTeacher(teacherId);

            IEnumerable<SubjectDTO> diff = allSubjects.Except(tauGhtSubjects).ToList();
            return diff;
        }
    }

       
        
    
}