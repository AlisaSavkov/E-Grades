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
using AutoMapper;
using NLog;
using Projekat.Filters;
using Projekat.Infrastructure;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Services;

namespace Projekat.Controllers
{
    [RoutePrefix("project/teachers")]
    public class TeachersController : ApiController
    {
        private ITeacherService teacherService;
        private ISubjectTeacherService subTeachService;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TeachersController(ITeacherService teacherService, ISubjectTeacherService subTeachService)
        {
            this.teacherService = teacherService;
            this.subTeachService = subTeachService;
        }

        // GET: api/Teachers
        [Route("")]
        [Authorize(Roles = "admins, teachers, parents, students")]
        public IHttpActionResult GetTeachers()
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            bool isAdmin = RequestContext.Principal.IsInRole("admins");
            IEnumerable<Teacher> teachers = teacherService.GetAllTeachers();

            if(RequestContext.Principal.IsInRole("admins"))
            {
                logger.Info("Admin with id " + userId + " is requesting all teachers.");
                return Ok(teachers.ToList().Select(Mapper.Map<Teacher, TeacherDTO>));
            }
            else if (RequestContext.Principal.IsInRole("teachers"))
            {
                logger.Info("Teacher with id " + userId + " is requesting all teachers.");
                return Ok(teachers.ToList().Select(Mapper.Map<Teacher, TeacherTDTO>));
            }
            else if (RequestContext.Principal.IsInRole("parents"))
            {
                logger.Info("Parent with id " + userId + " is requesting all teachers.");
                return Ok(teachers.ToList().Select(Mapper.Map<Teacher, TeacherPDTO>));
            }
            else
            {
                logger.Info("Student with id " + userId + " is requesting all teachers.");
                return Ok(teachers.ToList().Select(Mapper.Map<Teacher, TeacherPDTO>));
            }
           
        }

       
        [Authorize(Roles = "admins, teachers")]
        [Route("findBySubject/{subjectId}")]
        public HttpResponseMessage GetTeachersBySubject(int subjectId)
        {
            logger.Info("Requesting all teachers by subject.");
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;

            try
            {
                logger.Info("User with id " + userId + " is requesting teachers by subject id " + subjectId);
                IEnumerable<TeacherDTO> teachers = teacherService.GetTeachersBySubject(subjectId);
                return Request.CreateResponse(HttpStatusCode.OK,teachers);
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

           
                
        }

        
        // GET: api/Teachers/5
        [Authorize(Roles = "admins, teachers, parents, students")]
        [Route("{id}")]
        public HttpResponseMessage GetTeacher(string id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("User with id " + userId + " is requesting teacher by ID" + id);

           
            try
            {
                Teacher teacher = teacherService.GetById(id);
                
                    if (RequestContext.Principal.IsInRole("admins"))
                    {
                        logger.Info("Admin with id " + userId + " is requesting a teacher with id " + id);
                        return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Teacher, TeacherDTO>(teacher));

                    }
                    else if (RequestContext.Principal.IsInRole("teachers"))
                    {
                        logger.Info("Teacher with id " + userId + " is requesting a teacher with id " + id);
                        return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Teacher, TeacherTDTO>(teacher));
                    }
                    else if (RequestContext.Principal.IsInRole("parents"))
                    {
                        logger.Info("Parent with id " + userId + " is requesting a teacher with id " + id);
                        return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Teacher, TeacherPDTO>(teacher));
                    }
                    else
                    {
                        logger.Info("Student with id " + userId + " is requesting a teacher with id " + id);
                        return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Teacher, TeacherPDTO>(teacher));
                    }

                
            }
            catch(Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }
            
        }

        
        // PUT: api/Teachers/5
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(TeacherDTO))]
        [ValidateModel]
        [Route("update/{id}")]
        public HttpResponseMessage PutTeacher(string id, [FromBody]TeacherDTO dto)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("Admin with id " + userId + " is updating teacher by ID" + id);

            logger.Info("Updating a teacher.");
            if (dto == null || id != dto.ID)
            {
                logger.Info("Bad request.");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                TeacherDTO updated = teacherService.Update(id, dto);
                
                logger.Info("Teacher updated.");
                return Request.CreateResponse(HttpStatusCode.OK, updated);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }
        }


        // DELETE: api/Teachers/5
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(TeacherDTO))]
        [Route("{id}")]
        public HttpResponseMessage DeleteTeacher(string id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("Admin with id " + userId + " is deleting teacher by ID" + id);

            try
            {
                TeacherDTO removed = teacherService.Delete(id);
                logger.Info("Teacher deleted.");
                return Request.CreateResponse(HttpStatusCode.NoContent);
                }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Teacher deleted." + error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

       
        [Authorize(Roles = "admins")]
        [Route("{id}/add-subject/{subjectId}")]
        [ResponseType(typeof(SubjectTeacherDTO))]
        public HttpResponseMessage PostSubjectToTeacher(string id, int subjectId)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("Admin with id " + userId + " is deleting teacher by ID" + id);
            try
            {
                logger.Info("Creating subject-teacher.");
                SubjectTeacherDTO subjectTeacher = subTeachService.Create(id, subjectId);
               
                return Request.CreateResponse(HttpStatusCode.OK, subjectTeacher);
            }
            catch (Exception e)
            {
                logger.Info("Subject-teacher not created.");
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

        }

        
        [Authorize(Roles = "admins")]
        [Route("{id}/remove-subject/{subjectId}")]
        [ResponseType(typeof(void))]
        public HttpResponseMessage DeleteSubjectFromTeacher(string id, int subjectId)
        {
            try
            {
                SubjectTeacherDTO subTeacher = subTeachService.RemoveSubjectFromTeacher(id, subjectId);
                if (subTeacher == null)
                {
                    logger.Info("Subject or teacher not found.");
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                logger.Info("Subject-teacher deleted.");

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                logger.Info("Subject-teacher not deleted.");
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

        }

    }
}