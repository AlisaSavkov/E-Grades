using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projekat.Repositories
{
    public class GradesRepository: GenericRepository<Grade>, IGradesRepository
    {

        public GradesRepository(DbContext context) : base(context)

        {
        }

        public IEnumerable<Grade> GetByParentId(string parentId)
        {
            return Get().Where(x => x.Student.Parent.Id == parentId);
        }

        public IEnumerable<Grade> GetByStudentId(string studentId)
        {
            return Get().Where(x => x.Student.Id == studentId);
        }
        public IEnumerable<Grade> GetByTeacherId(string teacherId)
        {
            return Get().Where(x => x.ClassSubjectTeacher.SubjectTeacher.Teacher.Id == teacherId);
        }
        public IEnumerable<Grade> GetBySubjectTeacher(int subjectTeacherId)
        {
            return Get().Where(x => x.ClassSubjectTeacher.SubjectTeacher.ID == subjectTeacherId);
        }

        public IEnumerable<Grade> GetByClassSubjectTeacher(int cstId)
        {
           return Get().Where(x => x.ClassSubjectTeacher.ID == cstId);
        }

        public IEnumerable<Grade> GetByStudentSemester(string studentId, Semester semester)
        {
            return Get().Where(x => x.Student.Id == studentId && x.Semester == semester);
        }

        public IEnumerable<Grade> GetByTeacherIdStudentId(string teacherId, string studentId)
        {
            return Get().Where(x => x.Student.Id == studentId && x.ClassSubjectTeacher.SubjectTeacher.Teacher.Id == teacherId);
        }

        public IEnumerable<Grade> GetBySubject(int subjectId)
        {
            return Get().Where(x => x.ClassSubjectTeacher.SubjectTeacher.Subject.ID == subjectId);
        }

        public IEnumerable<Grade> GetGradesByStudentSubject(string studentId, int subjectId)
        {
            return Get().Where(x => x.ClassSubjectTeacher.SubjectTeacher.Subject.ID == subjectId && x.Student.Id == studentId);
        }

        public IEnumerable<Grade> getByClass(int id)
        {
            return  Get().Where(x => x.Student.Class.ID == id);
        }

        public IEnumerable<Grade> GetGradesByTeacherClass(string teacherId, int classId)
        {
            return Get().Where(x => x.Student.Class.ID == classId && x.ClassSubjectTeacher.SubjectTeacher.Teacher.Id == teacherId);
        }

        public IEnumerable<Grade> GetGradesByDate(DateTime startDate, DateTime endDate)
        {
            return Get().Where(x => x.GradeDate>= startDate && x.GradeDate <= endDate);
        }

        public IEnumerable<Grade> getGradesByDateTeacher(DateTime startDate, DateTime endDate, string userId)
        {
            return Get().Where(x => x.GradeDate >= startDate && x.GradeDate <= endDate && x.ClassSubjectTeacher.SubjectTeacher.Teacher.Id == userId);
        }

        public IEnumerable<Grade> getByStartName(string startName)
        {
            return Get().Where(x => x.Student.FirstName.ToLower().StartsWith(startName.ToLower()) || x.Student.LastName.ToLower().StartsWith(startName.ToLower()));
        }

        public Grade GetFinalSemesterSubjectStudent(Semester semester, int subjectId, string studentId)
        {
            if(semester == Semester.FIRST)
            {
                return Get().Where(x => x.Student.Id == studentId && x.ClassSubjectTeacher.SubjectTeacher.Subject.ID == subjectId && x.Semester == semester && x.GradeType == GradeType.HALFYEAR).FirstOrDefault();
            }
            else
            {
                return Get().Where(x => x.Student.Id == studentId && x.ClassSubjectTeacher.SubjectTeacher.Subject.ID == subjectId && x.Semester == semester && x.GradeType == GradeType.FINAL).FirstOrDefault();
            }
            
        }

        public IEnumerable<Grade> GetGradesByStudentClassSubjTeacher(string studentId, int classSubjectTeacherid)
        {
            return Get().Where(x => x.ClassSubjectTeacher.ID == classSubjectTeacherid && x.Student.Id == studentId);
        }

        public IEnumerable<Grade> getByTeacherStudentSubject(string userId, string studentId, int subjectId)
        {
            return Get().Where(x => x.Student.Id == studentId && x.ClassSubjectTeacher.SubjectTeacher.Teacher.Id == userId && x.ClassSubjectTeacher.SubjectTeacher.Subject.ID == subjectId);
        }
    }
}