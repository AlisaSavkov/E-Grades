using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using NLog;
using Projekat.ModelsIzmena.DTOs;
using Projekat.Filters;
using Projekat.Infrastructure;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Services;

namespace Projekat.Controllers
{
    [RoutePrefix("project/subjects")]
    public class SubjectsController : ApiController
    {
        private ISubjectService subjectService;
        private IStudentService studentService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SubjectsController(ISubjectService subjectService, IStudentService studentService)
        {
            this.subjectService = subjectService;
            this.studentService = studentService;
        }

        
        // GET: api/Subjects
        //[Authorize(Roles = "admins, teachers, parents, students")]
        [Route("")]
        public IEnumerable<SubjectDTO> Get()
        {
            logger.Info("Requesting all subjects");
            return subjectService.GetAllSubjects();
        }

        //provereno
        // GET: api/Subjects/5
        [Authorize(Roles = "admins, teachers, parents, students")]
        [ResponseType(typeof(SubjectDTO))]
        [Route("{id}")]
        public HttpResponseMessage GetSubject(int id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("User with id " + userId + " is requesting subject by ID" + id);
            try
            {
                SubjectDTO found = subjectService.GetDtoById(id);
                logger.Info("Subject with id " + id + " found.");
                return Request.CreateResponse(HttpStatusCode.OK, found);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Subject with id " + id + "not found." + error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

        //provereno
        [Authorize(Roles = "admins, teachers")]
        [Route("findByTeacher/{teacherId}")]
        public HttpResponseMessage GetSubjectsByTeacher(string teacherId)
        {
           
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            
            logger.Info("User with id " + userId + " is requesting subjects by teacher ID" + teacherId);
            try
            {
                IEnumerable<SubjectDTO> subjects = subjectService.GetSubjectsByTeacher(teacherId);
               
                logger.Info("Subjects by teacher are found.");
                return Request.CreateResponse(HttpStatusCode.OK, subjects); 
            }
            catch(Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Subjects by teacher not found." + error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }


        }

        [Authorize(Roles = "admins, teachers")]
        [Route("findByNoTeacher/{teacherId}")]
        public HttpResponseMessage GetNoSubjectsByTeacher(string teacherId)
        {

            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;

            logger.Info("User with id " + userId + " is requesting subjects not taught by teacher ID" + teacherId);
            try
            {
                IEnumerable<SubjectDTO> subjects = subjectService.GetSubjectsNoByTeacher(teacherId);

                logger.Info("Subjects not taught by teacher are found.");
                return Request.CreateResponse(HttpStatusCode.OK, subjects);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Subjects not taught by teacher not found." + error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }


        }

        //admin moze da vidi, teacheri mogu da vide, roditelji deteta mogu da vide i dete moze da vidi za sebe
        [Authorize(Roles = "admins, teachers, parents, students")]
        [Route("findByStudent/{studentId}")]
        public HttpResponseMessage GetSubjectsByStudent(string studentId)
        {
           
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("User with id " + userId + "  is requesting all subjects by student id " + studentId);
          
            try
            {
                Student student = studentService.GetById(studentId);
                
                    if (RequestContext.Principal.IsInRole("admins") || RequestContext.Principal.IsInRole("teachers") || userId == studentId
                        || userId == student.Id || userId == student.Parent.Id)
                    {
                        IEnumerable<SubjectDTO> subjects = subjectService.GetSubjectsByStudent(studentId);
                        
                        logger.Info("Subjects found.");
                        return Request.CreateResponse(HttpStatusCode.OK, subjects);
                    }
                    throw new Exception("User is not authorized to see subjects for student with id " + studentId);
              
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

          
        }

        

        //provereno
        // PUT: api/Subjects/5
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("update/{id}")]
        public HttpResponseMessage PutSubject(int id, SubjectUpdateDTO dto)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("Admin with id " + userId + "  is changing a subject by id " + id);

            if (dto == null || id != dto.ID)
            {
                logger.Info("Bad request.");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                SubjectDTO updated = subjectService.Update(id, dto);
               
                logger.Info("Subject with id " + id + " is updated.");
                return Request.CreateResponse(HttpStatusCode.OK, updated);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Subject with id " + id + " is not updated.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
            
        }

        //provereno
        // POST: api/Subjects
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("")]
        public HttpResponseMessage PostSubject(SubjectDTO dto)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("Admin with id " + userId + "  is inserting a new subject.");

            try
            {
                SubjectDTO inserted = subjectService.Create(dto);
                if (inserted == null)
                {
                    logger.Info("Bad request - subject is not added.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                return Request.CreateResponse(HttpStatusCode.Created, inserted);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Subject is not added.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
            
        }

        //provereno
        // DELETE: api/Subjects/5
        //ako se predmet predaje u nekom razredu ne moze da se obrise
        [Authorize(Roles = "admins")]
        [Route("{id}")]
        public HttpResponseMessage DeleteSubject(int id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("Admin with id " + userId + "  is inserting a new subject.");
            try
            {
                SubjectDTO removed = subjectService.Delete(id);

                logger.Info("Subject with id " + id +" is removed.");
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch(Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Subject with id " + id + " is not deleted.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
            
        }

    }
}