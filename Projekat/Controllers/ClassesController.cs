using NLog;

using Projekat.Models.DTOs;
using Projekat.Filters;
using Projekat.Models;
using Projekat.Repository;
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
    [RoutePrefix("project/classes")]
    public class ClassController : ApiController
    {
        private IClassService classService;
        private IClassSubjectTeacherService cstService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ClassController(IClassService classService, IClassSubjectTeacherService ctsService)
        {
            this.classService = classService;
            this.cstService = ctsService;
        }


        [Authorize(Roles = "admins, teachers, students, parents")]
        [Route("")]
        public IEnumerable<ClassDTO> GetClasses()
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + " is requesting all classes.");
            return classService.GetAllClasses();
        }

        [Authorize(Roles = "admins, teachers, students, parents")]
        [ResponseType(typeof(ClassDTO))]
        [Route("{id}")]
        public IHttpActionResult GetClass(int id)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + " is requesting class by ID " + id);
           
            ClassDTO found = classService.GetDtoById(id);
            if (found == null)
            {
                logger.Info("Class with id " + id + " not found.");
                return NotFound();
            }
            logger.Info("Class with id " + id + " found.");
            return Ok(found);
        }

       
        [Authorize(Roles = "admins, teachers")]
        [Route("teacherId/{teacherId}")]
        public IHttpActionResult GetClassessByTeacher(string teacherId)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("User with username " + " is requesting classes by teacher id " + teacherId);

            if (RequestContext.Principal.IsInRole("admins") || (RequestContext.Principal.IsInRole("teachers") && userId == teacherId))
            {

                IEnumerable<ClassDTO> classes = classService.GetAllClassesByTeacher(teacherId);
                return Ok(classes);
            }
            
            return BadRequest();
        }

        [Authorize(Roles = "admins, teachers")]
        [Route("teacherId/{teacherId}/subjectId/{subjectId}")]
        public HttpResponseMessage GetClassessByTeacherSubject(string teacherId, int subjectId)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("User with username " + " is requesting classes by teacher id " + teacherId + " and subject id " + subjectId);

            if (RequestContext.Principal.IsInRole("admins") || (RequestContext.Principal.IsInRole("teachers") && userId == teacherId))
            {

                IEnumerable<ClassDTO> classes = classService.GetAllClassesByTeacherSubject(teacherId, subjectId);
                return Request.CreateResponse(HttpStatusCode.OK, classes);
            }

            return  Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "admins")]
        [Route("")]
        [ValidateModel]
        [ResponseType(typeof(ClassDTO))]
        public HttpResponseMessage PostClass(ClassDTO classDto)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("Admin with username " + " is creating a class.");
            try
            {
                ClassDTO newClass = classService.Create(classDto);

                if (newClass == null)
                {
                    logger.Info("Class is not created.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                logger.Info("Class is created.");
                return Request.CreateResponse(HttpStatusCode.Created, newClass);
            }
            catch(Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Class with year " + classDto.Year + "and label " + classDto.Label + " already exists.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
            
        }

        [Authorize(Roles = "admins")]
        [Route("{id}")]
        [ValidateModel]
        [ResponseType(typeof(ClassDTO))]
        public HttpResponseMessage Put(int id, ClassDTO dto)
        {
            
            if (id != dto.ID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("Admin with username " + " is updating a class with id " + id);

            try
            {
                
                ClassDTO updated = classService.Update(id, dto);
                if(updated == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, updated);
                }
                logger.Info("Class is updated.");
                return Request.CreateResponse(HttpStatusCode.OK, updated);
            }
            catch(Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Class with year " + dto.Year + "and label " + dto.Label + " already exists.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
           

        }


        
        //ucenici
        [Authorize(Roles = "admins")]
        [Route("{id}")]
        [ResponseType(typeof(ClassDTO))]
        public HttpResponseMessage Delete(int id)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("Admin with username " + " is updating a class with id " + id);
            try
            {
                ClassDTO removed = classService.Delete(id);
                if (removed == null)
                {
                    logger.Info("Class is not found.");
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                logger.Info("Class is removed.");
                return Request.CreateResponse(HttpStatusCode.NoContent);

            }
            
             catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }


        }

        [Authorize(Roles = "admins")]
        [Route("{classId}/remove-subjectTeacher/{stId}")]
        [ResponseType(typeof(void))]
        public HttpResponseMessage DeleteSubjectFromTeacher(int classId, int stId)
        {
            try
            {
                ClassSubjectTeacherDTO cst = cstService.RemoveSubjectFromClass(classId, stId);
                if (cst == null)
                {
                    logger.Info("Subject, teacher or class not found.");
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                logger.Info("Class-subject-teacher deleted.");

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                logger.Info("Class-subject-teacher not deleted.");
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }

        }


    }

    
}
