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
    public class StudentService : IStudentService
    {

        private IUnitOfWork db;
        private IUserService userService;
        private IClassService classService;
        private IParentService parentService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public StudentService(IUnitOfWork db, IUserService userService, IClassService classService, IParentService parentService)
        {
            this.db = db;
            this.userService = userService;
            this.classService = classService;
            this.parentService = parentService;
            
        }

        public IEnumerable<StudentDTO> GetAllStudentsDTOs()
        {

            return db.StudentsRepository.Get().ToList().Select(Mapper.Map<Student, StudentDTO>);
        }

        public StudentDTO GetDtoById(string id)
        {
           
            Student found = GetById(id);
            return Mapper.Map<Student, StudentDTO>(found);
            
        }

        
        public StudentDTO Delete(string id)
        {
            Student removed = GetById(id);

            

            IEnumerable<Grade> grades = db.GradesRepository.GetByStudentId(id);
            if (grades.Count() > 0)
            {
                logger.Info("Can't delete student with grades.");
                throw new Exception("Can't delete student with grades!");
            }
            
            if (removed.Class != null)
            {
                removed.Class.Students.Remove(removed);
                removed.Parent.Children.Remove(removed);
                if (removed.Parent.Children.Count == 1)
                {
                    db.ParentsRepository.Delete(removed.Parent.Id);
                }
                db.StudentsRepository.Delete(id);
                db.Save();
                StudentDTO removedDto = Mapper.Map<Student, StudentDTO>(removed);

                return removedDto;
            }
            return null;
           

        }

        public StudentDTO AddImageToStudent(string id, string fileName)
        {
            Student student = GetById(id);
            if (student != null)
            {
                
                student.ImagePath = fileName;
                db.StudentsRepository.Update(student);
                db.Save();
                StudentDTO updated = Mapper.Map<Student, StudentDTO>(student);
                return updated;
            }
            return null;
        }

       
        public StudentDTO AddStudentToClass(string idStudent, int idClass)
        {
            Student foundStudent = GetById(idStudent);
            
            Class fClass = db.ClassesRepository.GetByID(idClass);
           
            if(foundStudent == null || fClass == null)
            {
                return null;
            }

           
            if(!fClass.Students.Contains(foundStudent))
            {
                if(foundStudent.Class != null && foundStudent.Class.Year == fClass.Year)
                {
                    foundStudent.Class.Students.Remove(foundStudent);
                }
                else
                {
                    throw new Exception("Can't put student in class from other year.");
                }
                
                fClass.Students.Add(foundStudent);
                foundStudent.Class = fClass;
                db.StudentsRepository.Update(foundStudent);
                StudentDTO studentDto = Mapper.Map<Student, StudentDTO>(foundStudent);
                db.Save();
                return studentDto;
            }
            logger.Info("Exception - student with id " + idStudent + " already attends the class with id " + idClass);
            throw new Exception("Student already attends that class");

        }

        public StudentDTO Update(string id, StudentDTO dto)
        {
            Student updated = GetById(id);
           
            if (updated != null)
            {

                if (dto.UserName != null && dto.UserName != updated.UserName && userService.GetByUserName(dto.UserName) == null)
                {
                    updated.UserName = dto.UserName;
                }
                //if (dto.Email != null && dto.Email != updated.Email && userService.GetByEmail(dto.Email) == null)
                //{
                //    updated.Email = dto.Email;
                //}
                if(dto.Email != null)
                {
                    updated.Email = dto.Email;
                }
                   
                if (dto.DateOfBirth != null)
                {
                    updated.DateOfBirth = dto.DateOfBirth;
                }
                if (dto.LastName != null)
                {
                    updated.LastName = dto.LastName;
                }

                if (dto.FirstName != null)
                {
                    updated.FirstName = dto.FirstName;
                }
               
                db.StudentsRepository.Update(updated);
                db.Save();
                return Mapper.Map<Student, StudentDTO>(updated);

            }

            return null;
        }

        public Student GetById(string id)
        {
            var user = userService.GetByUserID(id);
            if (user != null)
            {
                IList<string> roles = userService.FindRoles(id);
                if (!roles.Contains("students"))
                {
                    logger.Info("Exception - user is not a student.");
                    throw new Exception("User is not a student.");
                }
                else
                {
                    return db.StudentsRepository.GetByID(id);
                }
                
            }
            logger.Info("Exception - Student is not found.");
            throw new Exception("Student is not found.");

        }

    
        public IEnumerable<Student> GetByClassId(int classId)
        {
            Class clas = db.ClassesRepository.GetByID(classId);
            if (clas == null)
            {
                logger.Info("Exception - The class with id " + classId + " doesn't exist.");
                throw new Exception("The class with id " + classId + " doesn't exist.");
            }


            IEnumerable<Student> students = db.StudentsRepository.Get().Where(x => x.Class != null && x.Class.ID == classId);
            return students;
        }

        //get as admin
        public IEnumerable<StudentDTO> GetByParentId(string parentId)
        {
            Parent parent = parentService.GetById(parentId);
          
            return db.StudentsRepository.GetByParentId(parentId).ToList().Select(Mapper.Map<Student, StudentDTO>);
        }
        //get as parent
        public IEnumerable<StudentPDTO> GetByParentIdP(string parentId)
        {
            Parent parent = parentService.GetById(parentId);
           
            return db.StudentsRepository.GetByParentId(parentId).ToList().Select(Mapper.Map<Student, StudentPDTO>);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return db.StudentsRepository.Get();
        }

        public Student GetByUsername(string username)
        {
            try
            {
                Student found = db.StudentsRepository.GetByUsername(username);
                if (found == null)
                {
                    return null;
                }
                return found;
            }
            catch (Exception)
            {
                logger.Info("Exception - user with id " + username + " is not a student.");
                throw new Exception("User is not a student.");
            }
        }

        public IEnumerable<StudentPDTO> GetByClassIdTeacherIdSubjectId(int classId, string teacherId, int subjectId)
        {
            Class c = classService.GetById(classId);

            ClassSubjectTeacher cst = db.ClassSubjectTeachersRepository.GetByCST(classId, subjectId, teacherId);
            if(cst == null)
            {
                throw new Exception("Teacher with id is not teaching that subject in that class");
            }
            IEnumerable<Student> students = GetByClassId(cst.Class.ID);
            return students.ToList().Select(Mapper.Map<Student, StudentPDTO>); ;
           
        }
    }
    
}