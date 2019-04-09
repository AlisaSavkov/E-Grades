using AutoMapper;
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
    [RoutePrefix("project/parents")]
    public class ParentsController : ApiController
    {
        private IParentService parentsService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ParentsController(IParentService parentsService)
        {
            this.parentsService = parentsService;

        }

       

        [Authorize(Roles = "admins, teachers, parents, students")]
        [ResponseType(typeof(ParentDTO))]
        [Route("")]
        public IHttpActionResult GetParents()
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("Requesting all parents");
            IEnumerable<Parent> parents = parentsService.GetAllParents();
            if (RequestContext.Principal.IsInRole("admins"))
            {
                logger.Info("Admin with id " + userId + " is requesting all parents");
                return Ok(parents.ToList().Select(Mapper.Map<Parent, ParentDTO>));
            }
            else if (RequestContext.Principal.IsInRole("students"))
            {
                logger.Info("Admin with id " + userId + " is requesting all parents");
                return Ok(parents.ToList().Select(Mapper.Map<Parent, ParentPDTO>));
            }
            else if (RequestContext.Principal.IsInRole("parents"))
            {
                logger.Info("Admin with id " + userId + " is requesting all parents");
                return Ok(parents.ToList().Select(Mapper.Map<Parent, ParentPDTO>));
            }
            else
            {
                logger.Info("Teacher with id " + userId + " is requesting all parents");
                return Ok(parents.ToList().Select(Mapper.Map<Parent, ParentTDTO>));
            }


        }

       
        //admin moze da vidi sve podatke, teacher moze da vidi email, a parent i student samo ime i prezime
        [Authorize(Roles = "admins, teachers, parents, students")]
        [ResponseType(typeof(ParentDTO))]
        [Route("{id}")]
        public HttpResponseMessage GetParent(string id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;

            logger.Info("User with id " + userId + " is requesting parent by ID" + id);
            try
            {
                Parent parent = parentsService.GetById(id);
                //if (parent != null)
                //{
                if (RequestContext.Principal.IsInRole("admins"))
                {
                    logger.Info("Admin with id " + userId + " is requesting a parent with id " + id);
                    return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Parent, ParentDTO>(parent));

                }
                else if (RequestContext.Principal.IsInRole("teachers"))
                {
                    logger.Info("Teacher with id " + userId + " is requesting a parent with id " + id);
                    return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Parent, ParentTDTO>(parent));
                }
                else if (RequestContext.Principal.IsInRole("parents"))
                {
                    logger.Info("Parent with id " + userId + " is requesting a parent with id " + id);
                    return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Parent, ParentPDTO>(parent));
                }
                else
                {
                    logger.Info("Student with id " + userId + " is requesting a parent with id " + id);
                    return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Parent, ParentPDTO>(parent));
                }


            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

        [Route("username/{username}")]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetByUsername(string username)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("Admin with username " + userName + " is requesting parent by userame" + username);
            try
            {

                Parent parent = parentsService.GetByUserName(username);
                if (parent == null)
                {
                    logger.Info("Parent with username " + username + " not found admin by username" + userName);
                    return NotFound();
                }
                logger.Info("Parent with username " + username + " found admin by username" + userName);
                return Ok(Mapper.Map<Parent, ParentDTO>(parent));
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                ErrorDTO error = new ErrorDTO(e.Message);
                return BadRequest(error.MessageDetails);
            }
        }

        //[Route("jmbg/{jmbg}")]
        //[Authorize(Roles = "admins")]
        //public IHttpActionResult GetByJMBG(string jmbg)
        //{
        //    string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
        //    string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
        //    logger.Info("Admin with username " + userName + " is requesting parent by jmbg" + jmbg);
        //    try
        //    {

        //        Parent parent = parentsService.GetByJMBG(jmbg);
        //        if (parent == null)
        //        {
        //            logger.Info("Parent with jmbg " + jmbg + " not found admin by username" + userName);
        //            return NotFound();
        //        }
        //        logger.Info("Parent with jmbg " + jmbg + " found admin by username" + userName);
        //        return Ok(Mapper.Map<Parent, ParentDTO>(parent));
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Info(e.Message);
        //        ErrorDTO error = new ErrorDTO(e.Message);
        //        return BadRequest(error.WriteM());
        //    }
        //}

        [Route("jmbg/{jmbg}")]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetByJMBG([FromUri] string jmbg)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("Admin with username " + userName + " is requesting parent by jmbg" + jmbg);
            try
            {

                Parent parent = parentsService.GetByJMBG(jmbg);
                if (parent == null)
                {
                    logger.Info("Parent with jmbg " + jmbg + " not found admin by username" + userName);
                    return NotFound();
                }
                logger.Info("Parent with jmbg " + jmbg + " found admin by username" + userName);
                return Ok(Mapper.Map<Parent, ParentDTO>(parent));
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                ErrorDTO error = new ErrorDTO(e.Message);
                return BadRequest(error.MessageDetails);
            }
        }


       
        // PUT: api/Teachers/5
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(ParentDTO))]
        [ValidateModel]
        [Route("{id}")]
        public HttpResponseMessage PutParent(string id, ParentDTO dto)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;

            logger.Info("User with id " + userId + " is updating parent by ID" + id);
            
            if (dto == null || id != dto.ID)
            {
                logger.Info("Bad request.");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                ParentDTO updated = parentsService.Update(id, dto);
                if (updated == null)
                
                logger.Info("Parent updated.");
                return Request.CreateResponse(HttpStatusCode.OK, updated);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

        
        // DELETE: api/Teachers/5
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(ParentDTO))]
        [Route("{id}")]
        public HttpResponseMessage DeleteParent(string id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            logger.Info("User with id " + userId + " is deleting a parent by ID" + id);

            try
            {
                ParentDTO removed = parentsService.Delete(id);
               
                logger.Info("Parent removed.");
                return Request.CreateResponse(HttpStatusCode.OK, removed);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info("Parent not removed.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

    }
}
