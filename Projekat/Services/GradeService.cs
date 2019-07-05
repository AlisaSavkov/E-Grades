using AutoMapper;
using NLog;
using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Repository;
using Projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Services
{
    public class GradeService : IGradeService
    {

        private IUnitOfWork db;
        private IStudentService studentService;
        private ITeacherService teacherService;
        private ISubjectService subjectService;
        private ISubjectTeacherService subjectTeacherService;
        private IClassSubjectTeacherService cstService;
        private IEmailService emailService;
        private IParentService parentService;
        private IClassService classService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public GradeService(IUnitOfWork db, IStudentService studentService, ISubjectTeacherService subjectTeacherService,
            IEmailService emailService, IParentService parentService, ITeacherService teacherService, ISubjectService subjectService,
            IClassService classService, IClassSubjectTeacherService cstService)
        {
            this.db = db;
            this.studentService = studentService;
            this.subjectTeacherService = subjectTeacherService;
            this.emailService = emailService;
            this.parentService = parentService;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.classService = classService;
            this.cstService = cstService;
        }

        public GradeDTO Delete(int id)
        {
            Grade removed = GetById(id);

           
            GradeDTO removedDto = Mapper.Map<Grade, GradeDTO>(removed);
            removed.Student.Grades.Remove(removed);
            removed.ClassSubjectTeacher.Grades.Remove(removed);
            db.GradesRepository.Delete(id);
            db.Save();

            return removedDto;
           
        }

        public IEnumerable<GradeDTO> GetAllGrades()
        {
            return db.GradesRepository.Get().ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }

        public Grade GetById(int id)
        {

            Grade grade = db.GradesRepository.GetByID(id);
            if (grade == null)
            {
                logger.Info("Grade with id " + id + " not found.");
                throw new Exception("Grade with id " + id + " not found.");
            }
            return grade;
        }

        public GradeDTO GetDtoById(int id)
        {
            return Mapper.Map<GradeDTO>(GetById(id));
        }

        public IEnumerable<GradeDTO> GetGradesByStudent(string studentId)
        {
            return db.GradesRepository.GetByStudentId(studentId).ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }

      
        public GradeDTO Update(int id, GradeUpdateDTO gradeDto)
        {
            Grade grade = GetById(id);

            Semester semester = DateCheck();
            if (semester != grade.Semester)
            {
                throw new Exception("Can't change grade from other semester!");
            }

            grade.GradeValue = gradeDto.GradeValue;
            grade.Changed = true;

            db.GradesRepository.Update(grade);
            db.Save();


            return Mapper.Map<GradeDTO>(grade);
        }

       
        public GradeDTO Create(string studentId, string teacherId, int subjectId, GradeCreateDTO dto)
        {

            Student student = studentService.GetById(studentId);
           
            Subject subject = subjectService.GetById(subjectId);
            SubjectTeacher subjectTeacher = subjectTeacherService.GetBySubjectAndTeacher(subjectId, teacherId);
            if(subjectTeacher == null)
            {
                throw new Exception("Teacher is not teaching that subject.");
            }
            ClassSubjectTeacher cst = cstService.GetByClassSubjectTeacher(student.Class.ID, subjectTeacher.ID);
            
            if (dto != null)
            {
                

                Semester semester = DateCheck();
                
                Grade created = new Grade();
                created.GradeValue = dto.GradeValue;
                created.GradeDate = DateTime.Now;
                created.GradeType = GradeType.REGULAR;
                created.Year = student.Class.Year;
                created.Changed = false;
                created.Semester = semester;
                cst.Grades.Add(created);
                student.Grades.Add(created);
                db.GradesRepository.Insert(created);
                db.Save();
                string messageSubject = "Izvestaj o ocenjivanju";
                string body = CreateMessageBody(created);
                
                emailService.SendMail(messageSubject, body, student.Parent.Email);

                return Mapper.Map<GradeDTO>(created);
            }

            return null;


        }

       

        public IEnumerable<GradeDTO> GetGradesByStudentSemester(string studentId, Semester semester)
        {
            Student found = studentService.GetById(studentId);
            if (found == null)
            {
                throw new Exception("Student not found.");
            }
            return db.GradesRepository.GetByStudentSemester(studentId, semester).ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }

        public IEnumerable<GradeDTO> GetGradesByParent(string parentId)
        {
            Parent found = parentService.GetById(parentId);
            if (found == null)
            {
                throw new Exception("Parent not found.");
            }
            return db.GradesRepository.GetByParentId(parentId).ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }

        public IEnumerable<GradeDTO> GetGradesByTeacher(string teacherId)
        {
            Teacher found = teacherService.GetById(teacherId);
            if (found == null)
            {
                logger.Info("Teacher with id " + teacherId + " not found.");
                throw new Exception("Teacher not found.");
            }
            return db.GradesRepository.GetByTeacherId(teacherId).ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }

        public IEnumerable<GradeDTO> getByClass(int id)
        {
            Class found = classService.GetById(id);
            if (found == null)
            {
                logger.Info("Class with id " + id + " not found.");
                throw new Exception("Class not found.");
            }
            return db.GradesRepository.getByClass(id).ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }

        public IEnumerable<GradeDTO> GetGradesByTeacherClass(string teacherId, int classId)
        {
            Teacher teacher = teacherService.GetById(teacherId);
            Class found = classService.GetById(classId);
            if (found == null)
            {
                logger.Info("Class with id " + classId + " not found.");
                throw new Exception("Class not found.");
            }
            IEnumerable<ClassSubjectTeacher> csts = cstService.GetByTeacher(teacher.Id);

            if (csts.Where(x => x.Class.ID == classId).FirstOrDefault() != null)
            {
                IEnumerable<GradeDTO> grades = db.GradesRepository.GetGradesByTeacherClass(teacherId, classId).ToList().Select(Mapper.Map<Grade, GradeDTO>);

                return db.GradesRepository.GetGradesByTeacherClass(teacherId, classId).ToList().Select(Mapper.Map<Grade, GradeDTO>);
            }
            logger.Info("Class is not thaught by teacher with id " + teacherId);
            throw new Exception("Teacher  with id " + teacherId + " is not authorized to see grades for reqired class.");

        }
        public IEnumerable<GradeDTO> GetGradesByTeacherStudent(string teacherId, string studentId)
        {
            Teacher teacher = teacherService.GetById(teacherId);
            if (teacher == null)
            {
                logger.Info("Teacher with id " + teacherId + " not found.");
                throw new Exception("Teacher not found.");
            }
            Student student = studentService.GetById(studentId);
            if (student == null)
            {
                logger.Info("Student with id " + teacherId + " not found.");
                throw new Exception("Student not found.");
            }
            IEnumerable<ClassSubjectTeacher> csts = cstService.GetByTeacher(teacher.Id);

            if (csts.Where(x => x.Class.ID == student.Class.ID).FirstOrDefault() != null)
            {
                return db.GradesRepository.GetByTeacherIdStudentId(teacherId, studentId).ToList().Select(Mapper.Map<Grade, GradeDTO>);
            }
            else
            {
                logger.Info("Teacher with id " + teacherId + " doesn't teach the student with id " + studentId);
                throw new Exception("Teacher with id doesn't teach the student.");
            }
        }

        public IEnumerable<GradeDTO> GetGradesBySubject(int subjectId)
        {

            Subject subject = subjectService.GetById(subjectId);
           
            
            return db.GradesRepository.GetBySubject(subjectId).ToList().Select(Mapper.Map<Grade, GradeDTO>);

        }

       
        public Semester DateCheck()
        {
            DateTime today = DateTime.Now;
            DateTime lower1;
            DateTime lower2;
            DateTime upper1;
            DateTime upper2;
            if (today.Month >= 9 && today.Month <= 12)
            {
                lower1 = new DateTime(today.Year, 9, 1);
                upper1 = new DateTime(today.Year, 12, 21);
                lower2 = new DateTime(today.Year + 1, 1, 15);
                upper2 = new DateTime(today.Year + 1, 6, 14);
            }
            else if (today.Month >= 1 && today.Month <= 6)
            {
                lower1 = new DateTime(today.Year - 1, 9, 1);
                upper1 = new DateTime(today.Year - 1, 12, 21);
                lower2 = new DateTime(today.Year, 1, 15);
                upper2 = new DateTime(today.Year, 6, 14);
            }
            else
            {
                throw new Exception("Can not add a grade in the time o vacations!");
            }
            Grade grade = new Grade();
            if (today >= lower1 && today <= upper1)
            {
                return Semester.FIRST;
            }
            else if (today >= lower2 && today <= upper2)
            {
                grade.Semester = Semester.SECOND;
            }
            return Semester.SECOND;
        }

        public string CreateMessageBody(Grade created)
        {
            string body = "Poštovani," + "\n" + "Vaše dete " + created.Student.FirstName + " " + created.Student.LastName + " je dobilo ocenu " +
                    created.GradeValue + " iz predmeta " + created.ClassSubjectTeacher.SubjectTeacher.Subject.Name + " kod nastavnika " +
                    created.ClassSubjectTeacher.SubjectTeacher.Teacher.FirstName + " " + created.ClassSubjectTeacher.SubjectTeacher.Teacher.LastName + ".";

            return body;
        }

        public IEnumerable<GradeDTO> GetGradesByTeacherSubject(string teacherId, int subjectId)
        {
            SubjectTeacher st = subjectTeacherService.GetBySubjectAndTeacher(subjectId, teacherId);
            if(st == null)
            {
                return null;
            }
            return db.GradesRepository.GetBySubjectTeacher(st.ID).ToList().Select(Mapper.Map<Grade, GradeDTO>);

        }

        IEnumerable<GradeDTO> IGradeService.GetGradesBySubject(int subjectId)
        {
            Subject subject = subjectService.GetById(subjectId);
            //ova provera je mozda visak
            if (subject == null)
            {
                logger.Info("Subject with id " + subjectId + " not found.");
                throw new Exception("Subject not found.");
            }
            return db.GradesRepository.GetBySubject(subjectId).ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }

        public IEnumerable<GradeDTO> GetGradesByStudentSubject(string studentId, int subjectId)
        {
            Student student = studentService.GetById(studentId);
            Subject subject = subjectService.GetById(subjectId);
            IEnumerable<ClassSubjectTeacher> csts = cstService.GetByClass(student.Class.ID);
            
            if (csts.Where(x => x.SubjectTeacher.Subject.ID == subjectId).FirstOrDefault() != null)
            {
                IEnumerable<GradeDTO> grades = db.GradesRepository.GetGradesByStudentSubject(studentId, subjectId).ToList().Select(Mapper.Map<Grade, GradeDTO>);
                return grades;
            }

            logger.Info("Student with id " + studentId + " doesn't attend subject with id " + subjectId);
            throw new Exception("Student with id " + studentId + " doesn't attend subject with id " + subjectId);
        }

        public IEnumerable<GradeDTO> GetGradesByDate(DateTime startDate, DateTime endDate)
        {
            return db.GradesRepository.GetGradesByDate(startDate, endDate).ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }

        public IEnumerable<GradeDTO> GetGradesByDateTeacher(DateTime startDate, DateTime endDate, string userId)
        {
            return db.GradesRepository.getGradesByDateTeacher(startDate, endDate, userId).ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }

        public IEnumerable<GradeDTO> GetByStartName(string text)
        {

            return db.GradesRepository.getByStartName(text).ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }

        public IEnumerable<GradeDTO> GetByStartNameTeacherId(string startName, string id)
        {
            Teacher teacher = teacherService.GetById(id);
            return db.GradesRepository.getByStartName(startName).Where(x => x.ClassSubjectTeacher.SubjectTeacher.Teacher.Id == id).ToList().Select(Mapper.Map<Grade, GradeDTO>);
        }


        
       
        private Grade GetFinalSemesterSubjectStudent(Semester FIRST, int subjectId, string studentId)
        {
           return db.GradesRepository.GetFinalSemesterSubjectStudent(FIRST, subjectId, studentId);
        }

        public IEnumerable<GradeDTO> GetGradesByStudentClassSubjTeacher(string studentId, int classSubjectTeacherid)
        {
            IEnumerable<GradeDTO> grades = db.GradesRepository.GetGradesByStudentClassSubjTeacher(studentId, classSubjectTeacherid).ToList().Select(Mapper.Map<Grade, GradeDTO>);
            if (grades == null)
            {
                throw new Exception("Grades null");
            }
            return grades;
        }

        public IEnumerable<GradeDTO> GetGradesByTeacherStudentSemester(string teacherId, string studentId, Semester semester)
        {
            return GetGradesByTeacherStudent(teacherId, studentId).Where(x => x.Semester == semester);
        }

        public IEnumerable<GradeDTO> GetGradesByTeacherStudentSubject(string userId, string studentId, int subjectId)
        {
            return db.GradesRepository.getByTeacherStudentSubject(userId, studentId, subjectId).ToList().Select(Mapper.Map<Grade,GradeDTO>);
        }
    }
}