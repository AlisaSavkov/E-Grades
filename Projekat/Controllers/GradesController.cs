using AutoMapper;
using NLog;
using Projekat.Filters;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace Projekat.Controllers
{
    [RoutePrefix("project/grades")]
    public class GradesController : ApiController
    {
        private IGradeService gradeService;
        private IStudentService studentService;
        private ITeacherService teacherService;
        private ISubjectService subjectService;
        private ISubjectTeacherService subjectTeacherService;
        private IClassSubjectTeacherService cstService;
        private IParentService parentService;
        private IClassService classService;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public GradesController(IGradeService gradeService, IStudentService studentService, ITeacherService teacherService,
            ISubjectTeacherService subjectTeacherService, IClassSubjectTeacherService cstService, IParentService parentService,
            ISubjectService subjectService, IClassService classService)
        {
            this.gradeService = gradeService;
            this.studentService = studentService;
            this.teacherService = teacherService;
            this.subjectTeacherService = subjectTeacherService;
            this.cstService = cstService;
            this.parentService = parentService;
            this.subjectService = subjectService;
            this.classService = classService;
        }

        // GET: api/Grades
        [Authorize(Roles = "admins")]
        [Route("")]
        public IEnumerable<GradeDTO> GetGrades()
        {
            logger.Info("Requesting all grades.");
            return gradeService.GetAllGrades();
        }

       

        [Route("{id}")]
        [Authorize(Roles = "admins, teachers, students,parents")]
        [ResponseType(typeof(GradeDTO))]
        public HttpResponseMessage GetGradeById(int id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by grade id " + id);
            try
            {

                Grade grade = gradeService.GetById(id);
                if (RequestContext.Principal.IsInRole("admins") || grade.Student.Id == userId || grade.Student.Parent.Id == userId || grade.ClassSubjectTeacher.SubjectTeacher.Teacher.Id == userId)
                {
                    
                    GradeDTO dto = Mapper.Map<GradeDTO>(grade);

                    logger.Info("Grade with id " + id + " is found.");
                    return Request.CreateResponse(HttpStatusCode.OK, dto);
                }
                logger.Info("User with id " + userId + " is not authorized to see the grade with id " + id);
                throw new Exception("User is not authorized to see the grade!");

            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

       

        [Route("studentUsername/{username}")]
        [HttpGet]
        [Authorize(Roles = "admins, students, parents, teachers")]
        public IHttpActionResult GetByStudentUsername(string username)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by student id.");
            try
            {
                Student st = studentService.GetByUsername(username);
                if (st != null)
                {
                    if (RequestContext.Principal.IsInRole("admins") || st.UserName == username || st.Parent.Id == userId)
                    {
                        IEnumerable<GradeDTO> grades = gradeService.GetGradesByStudent(st.Id);
                        logger.Info("Grades by student id are found.");
                        return Ok(grades);
                    }
                    else if (RequestContext.Principal.IsInRole("teachers"))
                    {
                        IEnumerable<GradeDTO> grades = gradeService.GetGradesByTeacherStudent(userId, st.Id);
                        logger.Info("Grades by student id are found.");
                        return Ok(grades);
                    }

                    logger.Info("User with username " + userName + " is not authorized to see the grades for required student.");
                    return BadRequest("User is not authorized to see the grades!");
                }
                logger.Info("Exception - Student not found.");
                return BadRequest("Student with required username is not found.");
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return BadRequest(error.WriteM());
            }

        }

      
        [Route("student/{studentId}")]
        [Authorize(Roles = "admins, students, parents, teachers")]
        public IHttpActionResult GetGradesByStudent(string studentId)
        {

            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by student id.");
            try
            {
                Student st = studentService.GetById(studentId);
                if (st != null)
                {

                    if (RequestContext.Principal.IsInRole("admins") || st.Id == userId || (RequestContext.Principal.IsInRole("parents") && st.Parent.Id == userId))
                    {
                        IEnumerable<GradeDTO> grades = gradeService.GetGradesByStudent(studentId);
                        logger.Info("Grades by student id are found.");
                        return Ok(grades);

                    }
                    else if (RequestContext.Principal.IsInRole("teachers"))
                    {
                        IEnumerable<GradeDTO> grades = gradeService.GetGradesByTeacherStudent(userId, studentId);
                        logger.Info("Grades by student id are found.");
                        return Ok(grades);
                    }
                    logger.Info("User with username " + userName + " is not authorized to see the grades for required student.");
                    throw new Exception("User is not authorized to see the grades!");
                }
                logger.Info("Exception - Student not found.");
                throw new Exception("Student with required id is not found.");
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return BadRequest(error.WriteM());
            }

        }

        [Route("student/{studentId}/subject/{subjectId}")]
        [Authorize(Roles = "admins, parent, teachers")]
        public IHttpActionResult GetGradesByStudentSubject(string studentId, int subjectId)
        {

            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by student id.");
            try
            {
                Student st = studentService.GetById(studentId);
                if (st != null)
                {

                    if (RequestContext.Principal.IsInRole("admins") || ((RequestContext.Principal.IsInRole("parents") && st.Parent.Id == userId)))
                    {
                        IEnumerable<GradeDTO> grades = gradeService.GetGradesByStudentSubject(studentId, subjectId);
                        logger.Info("Grades by student id are found.");
                        return Ok(grades);

                    }
                    else if(RequestContext.Principal.IsInRole("teachers"))
                    {
                        ClassSubjectTeacherDTO cst = cstService.GetByCST(st.Class.ID, subjectId, userId);
                        if(cst != null)
                        {
                            IEnumerable<GradeDTO> grades = gradeService.GetGradesByTeacherStudentSubject(userId, studentId, subjectId);
                            logger.Info("Grades by student id and subject id are found.");
                            return Ok(grades);
                        }
                        logger.Info("Teacher with username " + userName + " is not authorized to see the grades for required student.");
                        throw new Exception("Teacher is not authorized to see the grades!");
                    }
                    logger.Info("User with username " + userName + " is not authorized to see the grades for required student.");
                    throw new Exception("User is not authorized to see the grades!");
                }
                logger.Info("Exception - Student not found.");
                throw new Exception("Student with required id is not found.");
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return BadRequest(error.MessageDetails);
            }

        }




        //prikaz svih ocena po roditelju
        [Route("parent/{parentId}")]
        [Authorize(Roles = "admins, parents")]
        public HttpResponseMessage GetGradesByParent(string parentId)
        {

            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by parent id.");
            try
            {

                Parent parent = parentService.GetById(parentId);
                if (parent != null)
                {

                    if (RequestContext.Principal.IsInRole("admins") || parent.Id == userId)
                    {
                        IEnumerable<GradeDTO> grades = gradeService.GetGradesByParent(parentId);
                        logger.Info("Grades by parent id are found.");
                        return Request.CreateResponse(HttpStatusCode.OK, grades);

                    }
                    logger.Info("User with username " + userName + " is not authorized to see the grades for required student.");
                    throw new Exception("User is not authorized to see the grades!");
                }
                logger.Info("Exception - Parent not found.");
                throw new Exception("Parent with required id is not found.");
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

        [Route("teacher/{teacherId}")]
        [Authorize(Roles = "admins, teachers, students")]
        public HttpResponseMessage GetGradesByTeacher(string teacherId)
        {

            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by teacher id.");


            try
            {
                Teacher teacher = teacherService.GetById(teacherId);
                if (teacher != null)
                {

                    if (RequestContext.Principal.IsInRole("admins") || teacher.Id == userId)
                    {
                        IEnumerable<GradeDTO> grades = gradeService.GetGradesByTeacher(teacherId);
                        logger.Info("Grades by teacher id are found.");
                        return Request.CreateResponse(HttpStatusCode.OK, grades);

                    }
                    else if (RequestContext.Principal.IsInRole("students"))
                    {
                        Student student = studentService.GetById(userId);

                        IEnumerable<GradeDTO> grades = gradeService.GetGradesByTeacherStudent(teacherId, student.Id);
                        logger.Info("Grades by student id are found.");
                        return Request.CreateResponse(HttpStatusCode.OK, grades);

                    }
                    logger.Info("User with username " + userName + " is not authorized to see the grades for required teacher.");
                    throw new Exception("User is not authorized to see the grades!");
                }
                logger.Info("Exception - Teacher not found.");
                throw new Exception("Teacher with required id is not found.");
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.MessageDetails);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

       
        [Route("subject/{subjectId}")]
        [Authorize(Roles = "admins, teachers, students")]
        public HttpResponseMessage GetGradesBySubject(int subjectId)
        {

            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by subject id.");

            try
            {
                Subject subject = subjectService.GetById(subjectId);
                if (subject != null)
                {

                    if (RequestContext.Principal.IsInRole("admins"))
                    {
                        IEnumerable<GradeDTO> grades = gradeService.GetGradesBySubject(subjectId);
                        logger.Info("Grades by subject id are found.");
                        return Request.CreateResponse(HttpStatusCode.OK, grades);

                    }
                    else if (RequestContext.Principal.IsInRole("students"))
                    {
                        
                        IEnumerable<GradeDTO> studentGrades = gradeService.GetGradesByStudentSubject(userId, subjectId);
                        if (studentGrades == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound);
                        }
                        logger.Info("Grades by subject id are found.");
                        return Request.CreateResponse(HttpStatusCode.OK, studentGrades);
                       
                    }
                    
                    else
                    {
                       
                        IEnumerable<GradeDTO> teacherGrades = gradeService.GetGradesByTeacherSubject(userId, subjectId);
                        if (teacherGrades == null)
                        {
                            logger.Info("Teacher that is loged-in doesn't teach the subject.");
                            throw new Exception("Teacher that is loged-in doesn't teach the subject.");

                        }
                        logger.Info("Grades by subject id are found.");
                        return Request.CreateResponse(HttpStatusCode.OK, teacherGrades);

                    }
                    
                }
                logger.Info("Exception - Subject not found.");
                throw new Exception("Subject with required id is not found.");
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

        
        [Authorize(Roles = "admins, students, teachers, parents")]
        [Route("student/{studentId}/semester")]
        [ValidateModel]
        public HttpResponseMessage GetGradesByStudentSemester(string studentId, [FromUri]Semester semester)
        {
            logger.Info("Requesting grades by student id and semester.");
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by student id and semster " + semester);

            try
            {
                Student st = studentService.GetById(studentId);

                if (RequestContext.Principal.IsInRole("admins") || st.Id == userId || st.Parent.Id == userId)
                {

                    IEnumerable<GradeDTO> grades = gradeService.GetGradesByStudentSemester(studentId, semester);
                    logger.Info("Grades by student id  and semester are found.");
                    return Request.CreateResponse(HttpStatusCode.OK, grades);
                }
                else
                {
                    IEnumerable<GradeDTO> gradesSemester = gradeService.GetGradesByTeacherStudentSemester(userId, st.Id, semester);

                    logger.Info("Grades by student id and semester are found.");
                    return Request.CreateResponse(HttpStatusCode.OK, gradesSemester);

                }
              
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

       

        [Route("findByDate/{startDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}/and/{endDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        //[Route("findByDate/{startDate}/and/{endDate}")]
        [Authorize(Roles = "admins, teachers")]
        public HttpResponseMessage GetGradesByDate(DateTime startDate, DateTime endDate)
        {

            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by  between date " + startDate + " and " + endDate);
            try
            {
               
                if (startDate > endDate)
                {
                    throw new Exception("Start date must be before end date!");
                }

                if (RequestContext.Principal.IsInRole("admins"))
                {
                    IEnumerable<GradeDTO> grades = gradeService.GetGradesByDate(startDate, endDate);
                    logger.Info("Grades by date are found by admin.");
                    return Request.CreateResponse(HttpStatusCode.OK, grades);
                }
                else
                {
                    IEnumerable<GradeDTO> grades = gradeService.GetGradesByDateTeacher(startDate, endDate, userId);
                    logger.Info("Grades by date are found by teacher.");
                    return Request.CreateResponse(HttpStatusCode.OK, grades);
                }
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

        
        [Route("findByClass/{classId}")]
        [Authorize(Roles = "admins, teachers")]
        public HttpResponseMessage GetGradesByClass(int classId)
        {
            
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by class id " + classId);

            try
            {
                if (RequestContext.Principal.IsInRole("admins"))
                {
                    IEnumerable<GradeDTO> grades = gradeService.getByClass(classId);
                    logger.Info("Grades by class id " + classId + " are found.");
                    return Request.CreateResponse(HttpStatusCode.OK, grades);
                }
                else
                {

                    IEnumerable<GradeDTO> grades = gradeService.GetGradesByTeacherClass(userId, classId);

                    logger.Info("Grades by class id are found.");
                    return Request.CreateResponse(HttpStatusCode.OK, grades);

                }
 
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

        
        //proverava da li ime ili prezime ucenika pocinje sa unetim stringom
        [Route("findByStartNameSurname")]
        [Authorize(Roles = "admins, teachers")]
        public HttpResponseMessage GetGradesByString([FromUri] string startNameSurname)
        {

            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            logger.Info("User with username " + userName + " is requesting grades by string in name or surname " + startNameSurname);

            try
            {

                if (RequestContext.Principal.IsInRole("admins"))
                {
                    IEnumerable<GradeDTO> grades = gradeService.GetByStartName(startNameSurname);
                    logger.Info("Admin with username " + userName + "gets grades by name or last name statring with string " + startNameSurname);
                    return Request.CreateResponse(HttpStatusCode.OK, grades);
                }
                else
                {
                   
                    IEnumerable<GradeDTO> grades = gradeService.GetByStartNameTeacherId(startNameSurname, userId);
                    logger.Info("Teacherwith username " + userName + "gets grades by name or last name statring with string " + startNameSurname);
                    return Request.CreateResponse(HttpStatusCode.OK, grades);
                }
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                logger.Info(error.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

      
        // POST: api/Grades
        [Authorize(Roles = "admins, teachers")]
        [Route("addStudent/{studentId}/addTeacher/{teacherId}/addSubject/{subjectId}")]
        [ValidateModel]
        public HttpResponseMessage Post(string studentId, string teacherId, int subjectId, [FromBody]GradeCreateDTO dto)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            try
            {

                if (RequestContext.Principal.IsInRole("admins") || (RequestContext.Principal.IsInRole("teachers") && userId == teacherId))
                {
                    logger.Info("User with username " + userName + " is adding a grade.");

                    GradeDTO grade = gradeService.Create(studentId, teacherId, subjectId, dto);
                    if (grade == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                    return Request.CreateResponse(HttpStatusCode.Created, grade);
                }

                logger.Info("User with username " + userName + " is not authorized to add new grades to required student for required subject-teacher!");
                throw new Exception("User with username " + userName + " is not authorized to add new grades to required student for required subject-teacher!");
               
            }
            catch (Exception e)
            {
                logger.Info("Grade is not created." + e.Message);
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }

        }

       


       
        //provereno
        // PUT: api/Grades/5
        [Authorize(Roles = "admins, teachers")]
        [Route("{id}")]
        [ValidateModel]
        public HttpResponseMessage Put(int id, [FromBody]GradeUpdateDTO gradeDto)
        {
            if (gradeDto == null || id != gradeDto.ID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            try
            {
                Grade grade = gradeService.GetById(id);

                if (RequestContext.Principal.IsInRole("admins") || (RequestContext.Principal.IsInRole("teachers") && grade.ClassSubjectTeacher.SubjectTeacher.Teacher.Id == userId))
                {
                    logger.Info("User with id " + userId + " is updating a grade with id " + id);
                    GradeDTO dto = gradeService.Update(id, gradeDto);

                    logger.Info("Grade with id " + id + " is updated.");
                    return Request.CreateResponse(HttpStatusCode.OK, dto);
                }
                logger.Info("Teacher with username " + userName + " not authorized to change grade with id " + id);
                throw new Exception("Teacher with username " + userName + " not authorized to change grade with id " + id);


            }
            catch (Exception e)
            {
                logger.Info("Grade not updated." + e.Message);
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

        //provereno
        [Authorize(Roles = "admins, teachers")]
        [Route("change/{id}")]
        [ValidateModel]
        [HttpPatch]
        public HttpResponseMessage PacthGrade(int id, [FromBody]GradeUpdateDTO gradeDto)
        {
            if (gradeDto == null || id != gradeDto.ID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string userName = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserName").Value;
            try
            {
                Grade grade = gradeService.GetById(id);

                if (RequestContext.Principal.IsInRole("admins") || (RequestContext.Principal.IsInRole("teachers") && grade.ClassSubjectTeacher.SubjectTeacher.Teacher.Id == userId))
                {
                    logger.Info("User with id " + userId + " is updating a grade with id " + id);
                    GradeDTO dto = gradeService.Update(id, gradeDto);

                    logger.Info("Grade with id " + id + " is updated.");
                    return Request.CreateResponse(HttpStatusCode.OK, dto);
                }
                logger.Info("Teacher with username " + userName + " not authorized to change grade with id " + id);
                throw new Exception("Teacher with username " + userName + " not authorized to change grade with id " + id);


            }
            catch (Exception e)
            {
                logger.Info("Grade not updated." + e.Message);
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
        }

        //provereno
        // DELETE: api/Grades/5
        [ResponseType(typeof(GradeDTO))]
        [Route("{id}")]
        [Authorize(Roles = "admins, teachers")]
        public HttpResponseMessage DeleteGrade(int id)
        {
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            bool isAdmin = RequestContext.Principal.IsInRole("admins");
           
            try
            {
                Grade grade = gradeService.GetById(id);
                if (grade.ClassSubjectTeacher.SubjectTeacher.Teacher.Id == userId || isAdmin)
                {
                    GradeDTO removed = gradeService.Delete(id);

                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }
                logger.Info("User not authorized to delete the grade.");
                throw new Exception("User not authorized to remove the grade.");
                
            }
            catch (Exception e)
            {
                ErrorDTO error = new ErrorDTO(e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error.MessageDetails);
            }
           
        }
    }
}
