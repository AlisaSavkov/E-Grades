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
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Projekat.Controllers
{
    [RoutePrefix("project/admins")]
    public class AdminsController : ApiController
    {
        private IAdminService adminService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AdminsController(IAdminService adminService)
        {
            this.adminService = adminService;

        }
       
        [Route("")]
        [Authorize(Roles = "admins, teachers")]
        public IHttpActionResult GetAll()
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("Admin with username " + userName + " is requesting all students");
            return Ok(adminService.GetAll());

        }
        
        [Route("{id}")]
        [Authorize(Roles = "admins, teachers")]
        public IHttpActionResult Get(string id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("Admin with username " + userName + " is requesting admin by ID" + id);
            try
            {
                AdminDTO admin = adminService.GetDtoById(id);
                logger.Info("Admin with username " + userName + " found admin by ID" + id);
                return Ok(admin);
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                ErrorDTO error = new ErrorDTO(e.Message);
                return BadRequest(error.MessageDetails);
            }
        }
        
        [Route("username/{username}")]
        [Authorize(Roles = "admins, teachers")]
        public IHttpActionResult GetByUsername(string username)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("Admin with username " + userName + " is requesting admin by userame" + username);
            try
            {

                AdminDTO admin = adminService.GetByUserName(username);
                if (admin == null)
                {
                    logger.Info("Admin with username " + userName + " not found admin by username" + username);
                    return NotFound();
                }
                logger.Info("Admin with username " + userName + " found admin by username" + username);
                return Ok(admin);
            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                ErrorDTO error = new ErrorDTO(e.Message);
                return  BadRequest(error.MessageDetails);
            }
        }

        
        [Route("{id}")]
        [Authorize(Roles = "admins")]
        public HttpResponseMessage Delete(string id)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
           
            
            try
            {
                if (userId == id)
                {
                    throw new Exception("Admin can not delete his own account!");

                }
                logger.Info("Admin with username " + userName + " is deleting admin with id " + id);
                AdminDTO removed = adminService.Delete(id);
                logger.Info("Admin with username " + userName + " removed admin with id " + id);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

       
        [ResponseType(typeof(AdminDTO))]
        [ValidateModel]
        [Route("{id}")]
        [Authorize(Roles = "admins")]
        public HttpResponseMessage Put(string id, [FromBody]AdminDTO dto)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("Admin with username " + userName + " is trying to update admin by ID " + id);

            if (dto == null || id != dto.ID)
            {
                logger.Info("Bad request.");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                AdminDTO updated = adminService.Update(id, dto);
                
                logger.Info("Admin with id " + id + " is updated.");
                return Request.CreateResponse(HttpStatusCode.OK, updated);
            }
            catch (Exception e)
            {
                
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

        
        [Authorize(Roles = "admins")]
        [HttpGet]
        [Route("logs")]
        public IHttpActionResult GetLogs()
        {
            StreamReader sr = null;
            List<string> logs = new List<string>();
            try
            {
                string root = HttpContext.Current.Server.MapPath("~/logs/app-log.txt");
                sr = new StreamReader(@root);
                while (true)
                {
                    string line = sr.ReadLine();

                    if (line == null)
                    {
                        break;
                    }
                    logs.Add(line);
                }
            }
            catch (IOException e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                return BadRequest(error.MessageDetails);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }

            return Ok(logs);
        }



        [HttpGet]
        [Authorize(Roles = "admins")]
        [Route("downloadlogs")]
        public HttpResponseMessage GetFormData()
        {

            string filename = HttpContext.Current.Server.MapPath("~/logs/app-log.txt");
            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();

                response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
                response.AddHeader("Content-Length", fileInfo.Length.ToString());
               // response.ContentType = "text/html";
                response.ContentType = "application/octet-stream";
                response.Flush();
                response.WriteFile(fileInfo.FullName);
                //response.TransmitFile(fileInfo.FullName);
                response.End();

               
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
               
            }

        }
    }
}
