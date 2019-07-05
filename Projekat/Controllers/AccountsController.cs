using Microsoft.AspNet.Identity;
using NLog;
using Projekat.Models.DTOs;
using Projekat.Filters;
using Projekat.Models.DTOs;
using Projekat.Repositories;
using Projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Projekat.Controllers
{
    [RoutePrefix("project/account")]
    public class AccountController : ApiController
    {
        private IUserRegistrationService service;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AccountController(IUserRegistrationService service, IUserService userService)
        {
            this.service = service;
            
        }

        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("register-teacher")]
        public async Task<IHttpActionResult> RegisterTeacher(TeacherRegistrationDTO user)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            try
            {
                var result = await service.RegisterTeacherUser(user);

                if (result == null)
                {

                    return BadRequest();
                }
                logger.Info("Admin with username " + userName + " registered a new teacher.");
                return Ok();
            }
            catch(Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(e.Message);
                return BadRequest(error.WriteM());
            }
            
        }

      
       

        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("register-student-parent")]
        public async Task<IHttpActionResult> RegisterStudentParent(SPRegistrationDTO dto)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            try
            {
                var result = await service.RegisterSPAsync(dto);

                if (result == null)
                {
                    return BadRequest();
                }
                logger.Info("Admin with username " + userName + " registered new student and parent.");
                return Ok();
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(e.Message);
                return BadRequest(error.MessageDetails);
            }
        }

        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("register-student")]
        public async Task<IHttpActionResult> RegisterStudent(StudentRegistrationDTO dto)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            try
            {
                var result = await service.RegisterStudent(dto);

                if (result == null)
                {
                    return BadRequest();
                }
                logger.Info("Admin with username " + userName + " registered new student.");
                return Ok();
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(e.Message);
                return BadRequest(error.MessageDetails);
            }
        }


        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("register-admin")]
        public async Task<IHttpActionResult> RegisterAdmin(AdminRegistrationDTO dto)
        {
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            try
            {
                var result = await service.RegisterAdminUser(dto);
                if (result == null)
                {
                    return BadRequest();
                }

                return Ok();
            }

            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(e.Message);
                return BadRequest(error.MessageDetails);
            }

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }
            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }
                return BadRequest(ModelState);
            }
            return null;
        }
    }
}
