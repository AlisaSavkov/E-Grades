using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NLog;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Repository;
using Projekat.Services;

namespace Projekat.Services
{
    public class SubjectTeacherService : ISubjectTeacherService
    {

        private IUnitOfWork db;
        private ITeacherService teacherService;
        private ISubjectService subjectService;
        private IClassService classService;
        private IUserService userService;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SubjectTeacherService(IUnitOfWork db, ITeacherService teacherService, ISubjectService subjectService, IUserService userService, IClassService classService
            )
        {
            this.db = db;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.userService = userService;
            this.classService = classService;
            
        }

        public SubjectTeacherDTO Create(string teacherId, int subjectId)
        {

            Teacher teacher = teacherService.GetById(teacherId);
            Subject subject = subjectService.GetById(subjectId);


            if (db.SubjectTeachersRepository.GetBySubjectTeacher(subjectId, teacherId) != null)
            {
                throw new Exception("Teacher already teaches that subject.");
            }
            SubjectTeacher teacherSubject = new SubjectTeacher();
            teacherSubject.Subject = subject;
            teacherSubject.Teacher = teacher;

            teacher.TaughtSubjects.Add(teacherSubject);
            subject.SubjectTeachers.Add(teacherSubject);

            db.SubjectTeachersRepository.Insert(teacherSubject);
            db.Save();

            return Mapper.Map<SubjectTeacherDTO>(teacherSubject);

        }

        public SubjectTeacherDTO Update(int id, int subjectId, string teacherId)
        {

            Teacher teacher = teacherService.GetById(teacherId);
            Subject subject = subjectService.GetById(subjectId);
            SubjectTeacher subjectTeacher = db.SubjectTeachersRepository.GetByID(id);

            if (subjectTeacher != null)
            {
                
                //izmena ako ne postoji takva kombinacija
                if (db.SubjectTeachersRepository.GetBySubjectTeacher(subjectId, teacherId) == null)
                {
                    
                    IEnumerable<ClassSubjectTeacher> csts = db.ClassSubjectTeachersRepository.GetBySubjectTeacher(id);
                    if (csts.Count() > 0)
                    {
                        if (subjectTeacher.Subject.Year != subject.Year)
                        {
                            throw new Exception("Can't change the subject because it's thaught in some classes that don 't attend subject for that year.");
                        }
                    }


                    subjectTeacher.Subject = subject;
                    subjectTeacher.Teacher = teacher;

                    teacher.TaughtSubjects.Add(subjectTeacher);
                    subject.SubjectTeachers.Add(subjectTeacher);

                    db.SubjectTeachersRepository.Update(subjectTeacher);
                    db.Save();

                    return Mapper.Map<SubjectTeacherDTO>(subjectTeacher);
                }
                //ako zapravo korisnik nista ne izmeni samo vrati isti taj objekat
                else if (subjectTeacher.Subject == subject && subjectTeacher.Teacher == teacher)
                {
                    return Mapper.Map<SubjectTeacherDTO>(subjectTeacher);
                }
                logger.Info("Exception - Teacher witd id " + teacherId + " already teaches subject with id " + subjectId);
                throw new Exception("Teacher already teaches that subject.");
                
            }
            logger.Info("Exception - Doesn't exist subject-teacher with required id.");
            throw new Exception("Doesn't exist subject-teacher with required id.");
        }



        public IEnumerable<SubjectTeacherDTO> GetAllSubjectTeachers()
        {
            return db.SubjectTeachersRepository.Get().ToList().Select(Mapper.Map<SubjectTeacher, SubjectTeacherDTO>);
        }

        public IEnumerable<SubjectTeacher> GetAllSubjectTeacher()
        {
            return db.SubjectTeachersRepository.Get();
        }

        public SubjectTeacherDTO GetDtoById(int Id)
        {
            return Mapper.Map<SubjectTeacherDTO>(db.SubjectTeachersRepository.GetByID(Id));
        }

        public SubjectTeacherDTO Delete(int id)
        {
            SubjectTeacher removed = db.SubjectTeachersRepository.GetByID(id);

            if (removed != null)
            {
                IEnumerable<ClassSubjectTeacher> csts = db.ClassSubjectTeachersRepository.GetBySubjectTeacher(id);
                if (csts.Count() > 0)
                {
                    logger.Info("Exception - Subject is taugt in some class. Subject-teacher with id " + id + " can't be deleted.");
                    throw new Exception("Subject is taugt in some class. Subject-teacher can't be deleted.");
                }
                Teacher teacher = removed.Teacher;
                Subject subject = removed.Subject;

                teacher.TaughtSubjects.Remove(removed);
                subject.SubjectTeachers.Remove(removed);
                db.SubjectTeachersRepository.Delete(removed);
                db.Save();
                return Mapper.Map<SubjectTeacher, SubjectTeacherDTO>(removed);
            }
            return null;

        }


        public SubjectTeacher GetBySubjectAndTeacher(int subjectId, string teacherId)
        {
            Teacher teacher = teacherService.GetById(teacherId);
            Subject subject = subjectService.GetById(subjectId);
            SubjectTeacher st = db.SubjectTeachersRepository.GetBySubjectTeacher(subjectId, teacherId);
            if (st == null)
            {
                return null;
            }
            return st;

        }

        public IEnumerable<SubjectTeacher> GetByTeacher(string teacherId)
        {
            IEnumerable<SubjectTeacher> sts = db.SubjectTeachersRepository.GetByTeacherId(teacherId);
            return sts;
        }

        public IEnumerable<SubjectTeacherDTO> GetByClass(int id)
        {
            Class clas = classService.GetById(id);

            IEnumerable<ClassSubjectTeacher> csts = db.ClassSubjectTeachersRepository.GetByClass(id);
            IList<SubjectTeacher> sts = new List<SubjectTeacher>();
            foreach (var item in csts)
            {
                if (item.Class.ID == id)
                {
                    sts.Add(item.SubjectTeacher);
                }
            }

            return sts.ToList().Select(Mapper.Map<SubjectTeacher, SubjectTeacherDTO>);


        }

        public SubjectTeacher GetById(int Id)
        {
            return db.SubjectTeachersRepository.GetByID(Id);
        }

        public SubjectTeacherDTO RemoveSubjectFromTeacher(string id, int subjectId)
        {
            SubjectTeacher st = GetBySubjectAndTeacher(subjectId, id);
            if(st == null)
            {
                logger.Info("Subject isn't taught by that teacher.");
                throw new Exception("Subject isn't taught by that teacher.");
            }

            IEnumerable<ClassSubjectTeacher> csts = db.ClassSubjectTeachersRepository.GetByTeacherSubject(id, subjectId);
                if (csts.Count() > 0)
                {
                    logger.Info("Exception - Subject is taugt in some class. Subject-teacher with id " + id + " can't be deleted.");
                    throw new Exception("Subject is taugt in some class. Subject-teacher can't be deleted.");
                }
                Teacher teacher = st.Teacher;
                Subject subject = st.Subject;

                teacher.TaughtSubjects.Remove(st);
                subject.SubjectTeachers.Remove(st);
                db.SubjectTeachersRepository.Delete(st);
                db.Save();
                return Mapper.Map<SubjectTeacher, SubjectTeacherDTO>(st);
           

        }
    }
}