using AutoMapper;
using NLog;
using Projekat.Filters;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Projekat.Controllers
{
    [RoutePrefix("project/students")]
    public class StudentsController : ApiController
    {

        private IStudentService studentService;
        private IEmailService emailService;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public StudentsController(IStudentService studentService, IEmailService emailService)
        {
            this.studentService = studentService;
            this.emailService = emailService;
        }

       

        [Route("")]
        [Authorize(Roles = "admins, teachers, parents, students")]
        public IHttpActionResult GetAll()
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            
            IEnumerable<Student> students = studentService.GetAllStudents();
            if(RequestContext.Principal.IsInRole("admins"))
            {
                logger.Info("Admin with username " + userName + " is requesting all students");
                return Ok(students.ToList().Select(Mapper.Map<Student, StudentPDTO>));
            }
            else if(RequestContext.Principal.IsInRole("teachers"))
            {
                logger.Info("Teacher with username " + userName + " is requesting all students");
                return Ok(students.ToList().Select(Mapper.Map<Student, StudentPDTO>));
            }
            else if (RequestContext.Principal.IsInRole("parents"))
            {
                logger.Info("Parent with username " + userName + " is requesting all students");
                return Ok(students.ToList().Select(Mapper.Map<Student, StudentPDTO>));
            }
            else
            {
                logger.Info("Student with username " + userName + " is requesting all students");
                return Ok(students.ToList().Select(Mapper.Map<Student, StudentPDTO>));
            }

        }

       
        //student i parent mogu iste podatke da vide
        [Route("class/{classId}")]
        [Authorize(Roles = "admins, teachers, parents, students")]
        public HttpResponseMessage GetByClass(int classId)
        {
            try
            {
                string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
                IEnumerable<Student> students = studentService.GetByClassId(classId);

                if (RequestContext.Principal.IsInRole("admins"))
                {
                    logger.Info("Admin with id " + userId + " is requesting all students by class id " + classId);
                    return Request.CreateResponse(HttpStatusCode.OK, students.ToList().Select(Mapper.Map<Student, StudentDTO>));
                   
                }
                else if (RequestContext.Principal.IsInRole("teachers"))
                {
                    logger.Info("Teacher with id " + userId + " is requesting all students");
                    return Request.CreateResponse(HttpStatusCode.OK, students.ToList().Select(Mapper.Map<Student, StudentPDTO>));
                }
                else if (RequestContext.Principal.IsInRole("parents"))
                {
                    logger.Info("Parent with id " + userId + "is requesting all students by class id " + classId);
                    return Request.CreateResponse(HttpStatusCode.OK, students.ToList().Select(Mapper.Map<Student, StudentPDTO>));
                }
                else
                {
                    logger.Info("Student with id " + userId + " is requesting all students by class id " + classId);
                    return Request.CreateResponse(HttpStatusCode.OK, students.ToList().Select(Mapper.Map<Student, StudentPDTO>));
                }

            }
            catch(Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
           
        }

        [Route("class/{classId}/teacher/{teacherId}/subject/{subjectId}")]
        [Authorize(Roles = "admins, teachers")]
        public HttpResponseMessage GetByClassTeacherSubject(int classId, string teacherId, int subjectId)
        {
            try
            {
                string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
                IEnumerable<StudentPDTO> students = studentService.GetByClassIdTeacherIdSubjectId(classId, teacherId, subjectId);

                if (RequestContext.Principal.IsInRole("admins"))
                {
                    logger.Info("Admin with id " + userId + " is requesting all students by class id " + classId);
                    return Request.CreateResponse(HttpStatusCode.OK, students);

                }
                else if (RequestContext.Principal.IsInRole("teachers") && userId ==teacherId)
                {
                    logger.Info("Teacher with id " + userId + " is requesting all students by class.");
                    return Request.CreateResponse(HttpStatusCode.OK, students);
                }
                
                else
                {
                    logger.Info("User with id " + userId + " is not autorized to see all students by class id " + classId);
                    throw new Exception("User is not authorized to see students in this class");
                }

            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

        //provereno
        [Route("parent/{parentId}")]
        [Authorize(Roles = "admins, parents")]
        public HttpResponseMessage GetByParent(string parentId)
        {
            logger.Info("Requesting all students by parent.");
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            bool isAdmin = RequestContext.Principal.IsInRole("admins");
            
            try
            {
                
                if (parentId == userId)
                {
                    logger.Info("Parent with id " + userId + " requested all students by parent.");
                    IEnumerable<StudentPDTO> students = studentService.GetByParentIdP(parentId);
                   
                    return Request.CreateResponse(HttpStatusCode.OK, students);
                }
                else if(isAdmin)
                {
                    logger.Info("Admin with id " + userId + " requested all students by parent.");
                    IEnumerable<StudentDTO> students = studentService.GetByParentId(parentId);
                   
                    return Request.CreateResponse(HttpStatusCode.OK, students);
                }

                logger.Info("User with id " + userId + " is not authorized!");
                throw new Exception("User is not authorized!");
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

     
       
        
        [Route("{id}")]
        [Authorize(Roles = "admins, parents, teachers, students")]
        public HttpResponseMessage Get(string id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
           
            logger.Info("User with id " + userId + " is requesting student by ID" + id);
            try
            {
                Student student = studentService.GetById(id);
                if (student != null)
                {
                    if (RequestContext.Principal.IsInRole("admins"))
                    {
                        logger.Info("Admin with id " + userId + " is requesting a student with id " + id);
                        return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Student, StudentDTO>(student));

                    }
                    else if (RequestContext.Principal.IsInRole("teachers"))
                    {
                        logger.Info("Teacher with id " + userId + " is requesting a student with id " + id);
                        return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Student, StudentPDTO>(student));
                    }
                    else if (RequestContext.Principal.IsInRole("parents") && student.Parent.Id == userId)
                    {
                        logger.Info("Parent with id " + userId + " is requesting a student with id " + id);
                        return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Student, StudentPDTO>(student));
                    }
                    else if (RequestContext.Principal.IsInRole("students") && student.Id == userId)
                    {
                        logger.Info("Student with id " + userId + " is requesting a student with id " + id);
                        return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Student, StudentPDTO>(student));
                    }
                    else { 

                        throw new Exception("User is not authorized to see the student.");
                    //{
                    //    logger.Info("Student with id " + userId + " is requesting a student with id " + id);
                    //    return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Student, StudentPDTO>(student));
                    }

                }
                logger.Info("Student with id " + id + " is not found.");

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                logger.Info("Bad request.");
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

        // provereno, ostala valdiacija datuma

        [ValidateModel]
        [Route("{id}")]
        [Authorize(Roles = "admins")]
        public HttpResponseMessage Put(string id, [FromBody]StudentDTO dto)
        {
            logger.Info("Updating student by ID " + id);
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            
            if (dto == null || id != dto.ID)
            {
                logger.Info("Bad request");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                StudentDTO updated = studentService.Update(id, dto);
                logger.Info("Admin with id " + userId + " updated a student by ID" + id);
                return Request.CreateResponse(HttpStatusCode.OK, updated);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Student with id " + id + " is not updated.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }
        //dodavanje slike uceniku
        [Route("upload-image/{id}")]
        [Authorize(Roles = "admins")]
        [ValidateModel]
        public async Task<HttpResponseMessage> PostImageFormData(string id)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

           string root = HttpContext.Current.Server.MapPath("~/App_Data");
            string fileSaveLocation = HttpContext.Current.Server.MapPath("~Images");
            //var provider = new MultipartFormDataStreamProvider(root);

            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
            List<string> files = new List<string>();


            try
            {

                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {

                    files.Add(Path.GetFileName(file.LocalFileName));

                    logger.Info("Server file path for inserted image: " + file.LocalFileName);
                    foreach (string f in files)
                    {
                        StudentDTO student = studentService.AddImageToStudent(id, file.LocalFileName);
                        logger.Info("Added an image to student with id " + id);
                        return Request.CreateResponse(HttpStatusCode.OK, student);
                    }
                    //StudentDTO student = studentService.AddImageToStudent(id, file.LocalFileName);

                   //if (student == null)
                   // {
                   //     logger.Info("Student with id " + id + " not found.");
                   //     return Request.CreateResponse(HttpStatusCode.NotFound);
                   // }
                    
                }
                logger.Info("Bad request.");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error.MessageDetails);
            }


        }

        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            }
        }

        //provereno
        [ResponseType(typeof(StudentDTO))]
       
        [Route("{studentId}/add-class/{classId}")]
        [Authorize(Roles = "admins")]
        public HttpResponseMessage PutStudentToClass(string studentId, int classId)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("Admin with username " + userName + " is putting a student with id " + studentId + " to class " + classId);
            try
            {
                StudentDTO updated = studentService.AddStudentToClass(studentId, classId);
                if (updated == null)
                {
                    logger.Info("Student not found.");
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                logger.Info("Student added to class.");
                return Request.CreateResponse(HttpStatusCode.OK, updated);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Student not added to class.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }


        }

        //provereno, ako ucenik ima ocen ne brise se
        // DELETE: api/Students/5
        
        [Route("{id}")]
        [Authorize(Roles = "admins")]
        public HttpResponseMessage Delete(string id)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;

            logger.Info("Admin with username " + userName + " is removing a student with id " + id);
            try
            {
                StudentDTO removed = studentService.Delete(id);
                
                logger.Info("Student removed.");
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch(Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

       
    }

   
    

}
