using NLog;
using Projekat.Filters;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace Projekat.Controllers
{
    [RoutePrefix("project/teacher-subjects")]
    public class SubjectTeachersController : ApiController
    {
        private ISubjectTeacherService subjectTeacherService;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public SubjectTeachersController(ISubjectTeacherService subjectTeacherService)
        {
            this.subjectTeacherService = subjectTeacherService;
        }

       
        [Route("")]
        [Authorize(Roles = "admins, teachers")]
        public IEnumerable<SubjectTeacherDTO> GetAll()
        {
            logger.Info("Requesting all teacher-subjects.");
            return subjectTeacherService.GetAllSubjectTeachers();
        }

       
        [ResponseType(typeof(SubjectTeacherDTO))]
        [Route("{id}")]
        [Authorize(Roles = "admins, teachers")]
        public IHttpActionResult GetTeacherSubject(int id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("User with id " + userId + "  is requesting  ateacher-subjects by ID");
           
            SubjectTeacherDTO found = subjectTeacherService.GetDtoById(id);
            if (found == null)
            {
                logger.Info("Subject-teacher is not found.");
                return NotFound();
            }
            logger.Info("Subject-teacher is found.");
            return Ok(found);
        }


        [ResponseType(typeof(SubjectTeacherDTO))]
        [Route("class/{id}")]
        [Authorize(Roles = "admins")]
        public HttpResponseMessage GetTeacherSubjectByClass(int id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("User with id " + userId + "  is requesting  teacher-subjects by ID");
            try
            {
                IEnumerable<SubjectTeacherDTO> found = subjectTeacherService.GetByClass(id);
               
                logger.Info("Subject-teachers are found.");
                return Request.CreateResponse(HttpStatusCode.OK, found);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

       
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(void))]
        [Route("{id}")]
        public HttpResponseMessage DeleteSubjectTeacher(int id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("Admin with id " + userId + "  is deleting  a subject-teacher by ID");
            try
            {
                SubjectTeacherDTO removed = subjectTeacherService.Delete(id);

                if (removed == null)
                {
                    logger.Info("Subject-teacher not found.");
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                logger.Info("Subject-teacher is deleted.");
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch(Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Subject-teacher is not deleted.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }
        }


        
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(SubjectDTO))]
        [Route("{id}/change-subject/{subjectId}/change-teacher/{teacherId}")]
        public HttpResponseMessage PutSubjectTeacher(int id, int subjectId, string teacherId)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("Admin with id " + userId + "  is updating  a subject-teacher.");
            try
            {
                SubjectTeacherDTO updated = subjectTeacherService.Update(id, subjectId, teacherId);

                if (updated == null)
                {
                   
                    logger.Info("Subject-teacher not found.");
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                logger.Info("Subject-teacher is updated.");
                return Request.CreateResponse(HttpStatusCode.OK, updated);
            }
            catch (Exception e)
            {

                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Subject-teacher is not updated.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }


        }
    }
}
