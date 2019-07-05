using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using Projekat.Models.DTOs;
using Projekat.Models;

using Projekat.Repository;

namespace Projekat.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {

        private IUnitOfWork db;
        private IUserService userService;
        private IEmailService emailService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public UserRegistrationService(IUnitOfWork unitOfWork, IUserService userService, IEmailService emailService)
        {
            db = unitOfWork;
            this.userService = userService;
            this.emailService = emailService;
            
        }

        public async Task<IdentityResult> RegisterAdminUser(AdminRegistrationDTO userDto)
        {

            if (await db.AuthRepository.FindByUserName(userDto.UserName) != null)
            {
                logger.Info("Exception - User with " + userDto.UserName + " already exists.");
                throw new Exception("User with " + userDto.UserName + " already exists.");
            }

            Admin user = Mapper.Map<AdminRegistrationDTO, Admin>(userDto);
            Task<IdentityResult> regisetredAdmin = db.AuthRepository.RegisterAdminUser(user, userDto.Password);
            string message = "Poštovani " + user.FirstName + " " + user.LastName + ", \n" +
                    "Registrovani ste  kao administrator na elektronski dnevnik. \nVaše korisničko ime je " + user.UserName + ".\nVaša lozinka je " +
                    userDto.Password;
            emailService.SendMail("Registracija", message, user.Email);

            return await regisetredAdmin;
            
        }

        public async Task<IdentityResult> RegisterTeacherUser(TeacherRegistrationDTO teacherDto)
        {
            if (await db.AuthRepository.FindByUserName(teacherDto.UserName) != null)
            {
                logger.Info("Exception - User with " + teacherDto.UserName + " already exists.");
                throw new Exception("User with that username already exists!");
            }
            if (teacherDto.JMBG != null && db.TeachersRepository.Get().Where(x => x.JMBG == teacherDto.JMBG).FirstOrDefault() != null)
            {
                logger.Info("Exception - Teacher with that JMBG already exists.");
                throw new Exception("Teacher with that JMBG already exists!");
            }
            Teacher user = Mapper.Map<TeacherRegistrationDTO, Teacher>(teacherDto);

            
            //return await db.AuthRepository.RegisterTeacherUser(user, teacherDto.Password);
            Task<IdentityResult> regTeacher = db.AuthRepository.RegisterTeacherUser(user, teacherDto.Password);
            string message1 = "Poštovani " + user.FirstName + " " + user.LastName + ",\n " +
                    "Registrovani ste na elektronski dnevnik kao nastavnik.\nVaše korisničko ime je " + user.UserName + ".\nVaša lozinka je " +
                    teacherDto.Password;
            emailService.SendMail("Registracija", message1, user.Email);
            return await regTeacher;
        }

        public async Task<IdentityResult> RegisterStudentParent(StudentParentRegistrationDTO studentParent)
        {
            
            
            if (await db.AuthRepository.FindByUserName(studentParent.Student.UserName) != null)
            {
                logger.Info("Exception - User with " + studentParent.Student.UserName + " already exists.");
                throw new Exception("User with that username already exists!");
            }

           
            Class c = db.ClassesRepository.GetByID(studentParent.Student.classID);
            if (c == null)
            {
                logger.Info("Exception - Class with " + studentParent.Student.classID + " doesn't exist.");
                throw new Exception("Class doesn't exist.");
            }
            Student student = new Student
            {
                UserName = studentParent.Student.UserName,
                FirstName = studentParent.Student.FirstName,
                LastName = studentParent.Student.LastName,
                Email = studentParent.Student.Email,
                DateOfBirth = studentParent.Student.DateOfBirth,
                
            };
            
            //ako postoji user sa usernamemo za roditelja proverimo da li je taj user roditelj
            var user = db.UsersRepository.Get().Where(x => x.UserName == studentParent.Parent.UserName).FirstOrDefault();
            if (user != null)
            {
                IList<string> roles = userService.FindRoles(user.Id);
                if (!roles.Contains("parents"))
                {
                    logger.Info("Exception - Username doesn't belong to a parent.");
                    throw new Exception("Username doesn't belong to a parent.");
                }
                
                Parent parent = db.ParentsRepository.Get().Where(x => x.UserName == studentParent.Parent.UserName).FirstOrDefault();
                
                student.Parent = parent;
                parent.Children.Add(student);
                c.Students.Add(student);
                student.Class = c;
                Task<IdentityResult> registeredStudent = db.AuthRepository.RegisterStudentUser(student, studentParent.Student.Password);
                string message = "Poštovani " + student.FirstName + " " + student.LastName + ",\n " +
                    "Registrovani ste na elektronski dnevnik kao učenik.\nVaše korisničko ime je " + student.UserName + ".\nVaša lozinka je " +
                    studentParent.Student.Password;
                emailService.SendMail("Registracija", message, student.Email);

                return await registeredStudent;
            }

            await RegisterParentUser(studentParent.Parent);
            Parent registered = db.ParentsRepository.Get().Where(x => x.UserName == studentParent.Parent.UserName).FirstOrDefault();
           
            registered.Children.Add(student);
            c.Students.Add(student);
            student.Class = c;
            Task<IdentityResult> regStudent= db.AuthRepository.RegisterStudentUser(student, studentParent.Student.Password);
            string message1 = "Poštovani " + student.FirstName + " " + student.LastName + ",\n " +
                    "Registrovani ste na elektronski dnevnik kao učenik. \nVaše korisničko ime je " + student.UserName + ".\nVaša lozinka je " +
                    studentParent.Student.Password;
            emailService.SendMail("Registracija", message1, student.Email);
            return await regStudent;
        }


        public async Task<IdentityResult> RegisterParentUser(ParentRegistrationDTO dto)
        {
            if (dto.JMBG != null && db.ParentsRepository.Get().Where(x => x.JMBG == dto.JMBG).FirstOrDefault() != null)
            {
                logger.Info("Exception - Parent with that JMBG already exists.");
                throw new Exception("Parent with that JMBG already exists!");
            }
            Parent user = new Parent
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                JMBG = dto.JMBG

            };
            Task<IdentityResult> registeredParent = db.AuthRepository.RegisterParentUser(user, dto.Password);
            string message = "Poštovani " + user.FirstName + " " + user.LastName + ", \n" +
                    "Registrovani ste  kao roditelj na elektronski dnevnik. \nVaše korisničko ime je " + user.UserName + ".\nVaša lozinka je " +
                    dto.Password;
            emailService.SendMail("Registracija", message, user.Email);

            return await registeredParent;
        }

        public async Task<IdentityResult> RegisterStudent(StudentRegistrationDTO dto)
        {
            if (await db.AuthRepository.FindByUserName(dto.UserName) != null)
            {
                logger.Info("Exception - User with " + dto.UserName + " already exists.");
                throw new Exception("User with " + dto.UserName + " already exists.");
            }

            Class c = db.ClassesRepository.GetByID(dto.classID);
            if (c == null)
            {
                logger.Info("Exception - Class with " + dto.classID + " doesn't exist.");
                throw new Exception("Class doesn't exist.");
            }
            Student user = new Student
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth

            };
            c.Students.Add(user);
            user.Class = c;

            Task<IdentityResult> registeredStudent = db.AuthRepository.RegisterStudentUser(user, dto.Password);
            string message = "Poštovani " + user.FirstName + " " + user.LastName + ", \n" +
                    "Registrovani ste  kao učenik na elektronski dnevnik. \nVaše korisničko ime je " + user.UserName + ".\nVaša lozinka je " +
                    dto.Password;
            emailService.SendMail("Registracija", message, user.Email);

            return await registeredStudent;
        }

        public async Task<IdentityResult> RegisterSPAsync(SPRegistrationDTO dto)
        {
            if (await db.AuthRepository.FindByUserName(dto.UserName) != null)
            {
                logger.Info("Exception - User with " + dto.UserName + " already exists.");
                throw new Exception("User with username " + dto.UserName+" already exists!");
            }

            Class c = db.ClassesRepository.GetByID(dto.classID);
            if (c == null)
            {
                logger.Info("Exception - Class with " + dto.classID + " doesn't exist.");
                throw new Exception("Class doesn't exist.");
            }
            Student student = new Student
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,

            };

            //ako postoji user sa usernamemo za roditelja proverimo da li je taj user roditelj
            var user = db.UsersRepository.Get().Where(x => x.UserName == dto.PUserName).FirstOrDefault();
            if (user != null)
            {
                IList<string> roles = userService.FindRoles(user.Id);
                if (!roles.Contains("parents"))
                {
                    logger.Info("Exception - Username doesn't belong to a parent.");
                    throw new Exception("Username " + dto.PUserName+ " doesn't belong to a parent.");
                }

                Parent parent = db.ParentsRepository.Get().Where(x => x.UserName == dto.PUserName).FirstOrDefault();

                student.Parent = parent;
                parent.Children.Add(student);
                c.Students.Add(student);
                student.Class = c;
                Task<IdentityResult> registeredStudent = db.AuthRepository.RegisterStudentUser(student, dto.Password);
                string message = "Poštovani " + student.FirstName + " " + student.LastName + ",\n " +
                    "Registrovani ste na elektronski dnevnik kao učenik.\nVaše korisničko ime je " + student.UserName + ".\nVaša lozinka je " +
                    dto.Password;
                emailService.SendMail("Registracija", message, student.Email);

                return await registeredStudent;
            }
           
            await RegisterPUser(dto);
            Parent registered = db.ParentsRepository.Get().Where(x => x.UserName == dto.PUserName).FirstOrDefault();

            registered.Children.Add(student);
            c.Students.Add(student);
            student.Class = c;
            Task<IdentityResult> regStudent = db.AuthRepository.RegisterStudentUser(student, dto.Password);
            string message1 = "Poštovani " + student.FirstName + " " + student.LastName + ",\n " +
                    "Registrovani ste na elektronski dnevnik kao učenik. \nVaše korisničko ime je " + student.UserName + ".\nVaša lozinka je " +
                    dto.Password;
            emailService.SendMail("Registracija", message1, student.Email);
            return await regStudent;
        }

        public async Task<IdentityResult> RegisterPUser(SPRegistrationDTO dto)
        {
            if (dto.JMBG != null && db.ParentsRepository.Get().Where(x => x.JMBG == dto.JMBG).FirstOrDefault() != null)
            {
                logger.Info("Exception - Parent with that JMBG already exists.");
                throw new Exception("Parent with that JMBG already exists!");
            }
            Parent user = new Parent
            {
                UserName = dto.PUserName,
                FirstName = dto.PFirstName,
                LastName = dto.PLastName,
                Email = dto.PEmail,
                JMBG = dto.JMBG

            };
            Task<IdentityResult> registeredParent = db.AuthRepository.RegisterParentUser(user, dto.PPassword);
            string message = "Poštovani " + user.FirstName + " " + user.LastName + ", \n" +
                    "Registrovani ste  kao roditelj na elektronski dnevnik. \nVaše korisničko ime je " + user.UserName + ".\nVaša lozinka je " +
                    dto.PPassword;
            emailService.SendMail("Registracija", message, user.Email);

            return await registeredParent;
        }
    }
}