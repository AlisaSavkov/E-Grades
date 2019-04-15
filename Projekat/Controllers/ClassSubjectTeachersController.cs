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
    [RoutePrefix("project/class-subject-teachers")]
    public class ClassSubjectTeachersController : ApiController
    {
        private IClassSubjectTeacherService cstService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ClassSubjectTeachersController(IClassSubjectTeacherService cstService)
        {
            this.cstService = cstService;
        }

        // GET: api/ClassSubjectTeachers
        [Authorize(Roles = "admins, teachers")]
        [Route("")]
        public IEnumerable<ClassSubjectTeacherDTO> GetClassSubjectTeachers()
        {
            logger.Info("Requesting all class-teacher-subjects.");
            return cstService.GetAllDTOs();
        }

       
        // GET: api/ClassSubjectTeachers/5
        [Authorize(Roles = "admins, teachers")]
        [Route("{id}")]
        [ResponseType(typeof(ClassSubjectTeacherDTO))]
        public IHttpActionResult GetClassSubjectTeacher(int id)
        {
            logger.Info("Requesting class-subject-teacher by id");
            ClassSubjectTeacherDTO classSubjectTeacher = cstService.GetDtoById(id);
            if (classSubjectTeacher == null)
            {
                logger.Info("Class-subject-teacher with id " + id + " doesn't exist.");
                return NotFound();
            }
            logger.Info("Class-subject-teacher with id " + id + " found.");
            return Ok(classSubjectTeacher);
        }


        [Authorize(Roles = "admins, teachers")]
        [Route("{id}")]
        [ResponseType(typeof(ClassSubjectTeacherDTO))]
        public IHttpActionResult GetByClassSubjectTeacher(int classId, int subjectId, string teacherId)
        {
            logger.Info("Requesting class-subject-teacher by id");
            ClassSubjectTeacherDTO classSubjectTeacher = cstService.GetByCST(classId, subjectId, teacherId);
            if (classSubjectTeacher == null)
            {
                logger.Info("Class-subject-teacher with doesn't exist.");
                return NotFound();
            }
            logger.Info("Class-subject-teacher is found.");
            return Ok(classSubjectTeacher);
        }


        [Authorize(Roles = "admins, teachers")]
        [Route("teacher/{id}")]
        [ResponseType(typeof(ClassSubjectTeacherDTO))]
        public IHttpActionResult GetClassSubjectTeacherByTeacher(string id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            if (RequestContext.Principal.IsInRole("admins") || (RequestContext.Principal.IsInRole("teachers") && userId == id))
            {
                logger.Info("Requesting class-subject-teachers by teacher id");
                IEnumerable<ClassSubjectTeacherDTO> ctss = cstService.GetByTeacher(id).ToList().Select(Mapper.Map<ClassSubjectTeacher, ClassSubjectTeacherDTO>);
                return Ok(ctss);
            }
            return BadRequest();
                
        }



        // PUT: api/ClassSubjectTeachers/5
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(void))]
        [Route("{id}/changeTeacher/{teacherId}/changeSubject/{subjectId}/changeClass/{classId}")]
        public HttpResponseMessage PutClassSubjectTeacher(int id, string teacherId, int subjectId, int classId)
        {
            try
            {
                logger.Info("Updating subject-teacher-class.");
                ClassSubjectTeacherDTO updated = cstService.Update(id, teacherId, subjectId, classId);
                if (updated == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, updated);
            }
            catch (Exception e)
            {
                logger.Info("Subject-teacher-class not updated.");
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

        //[ResponseType(typeof(void))]
        //[Route("{id}")]
        //[ValidateModel]
        //public HttpResponseMessage PutClassSubjectTeacher(int id, ClassSubjectTeacherUpdateDTO dto)
        //{
        //    if(dto == null || id!= dto.ID)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //    try
        //    {
        //        logger.Info("Updating subject-teacher-class.");
        //        ClassSubjectTeacherDTO updated = cstService.Update1(id, dto);
        //        if (updated == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound);
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, updated);
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Info("Subject-teacher-class not updated.");
        //        ErrorDTO error = new ErrorDTO(e.Message);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, error);
        //    }
        //}

        //// POST: api/ClassSubjectTeachers
        ///dodaj teacher-subject classi
        ///ako teacher-subject ne postoji, nemoj kreirati
        ///ako teacher subject postoji proveri da li tom odeljenju vec neko predaje taj predmet
        /////ako ga predaje ne dozvoliti kreiranje
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(ClassSubjectTeacherDTO))]
        [Route("addTeacher/{teacherId}/addSubject/{subjectId}/addClass/{classId}")]
        public HttpResponseMessage PostClassSubjectTeacher(string teacherId, int subjectId, int classId)
        {
            try
            {
                logger.Info("Creating subject-teacher-class.");
                ClassSubjectTeacherDTO created = cstService.Create(teacherId, subjectId, classId);
                if (created == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                logger.Info("Subject-teacher-class not created.");
                return Request.CreateResponse(HttpStatusCode.Created, created);
            }
            catch (Exception e)
            {
                logger.Info("Subject-teacher-class not created.");
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }


        [Authorize(Roles = "admins")]
        [ResponseType(typeof(ClassSubjectTeacherDTO))]
        [Route("addSubjectTeacher/{subTeacher}/addClass/{classId}")]
        public HttpResponseMessage PostClassSubjectTeacher(int subTeacher, int classId)
        {
            try
            {
                logger.Info("Creating subject-teacher-class.");
                ClassSubjectTeacherDTO created = cstService.Create1(subTeacher, classId);
                if (created == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                logger.Info("Subject-teacher-class not created.");
                return Request.CreateResponse(HttpStatusCode.Created, created);
            }
            catch (Exception e)
            {
                logger.Info("Subject-teacher-class not created.");
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

        // DELETE: api/ClassSubjectTeachers/5
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(ClassSubjectTeacher))]
        [Route("{id}")]
        public HttpResponseMessage DeleteClassSubjectTeacher(int id)
        {
            try
            {
                ClassSubjectTeacherDTO classSubjectTeacher = cstService.Delete(id);
                if (classSubjectTeacher == null)
                {
                    logger.Info("Subject-teacher-class with id " + id + "  not found.");
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                logger.Info("Subject-teacher-class with id " +  id + " deleted.");
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch(Exception e)
            {
                logger.Info("Subject-teacher-class with id " + id + " not deleted.");
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

     

    }
}