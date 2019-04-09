using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Repositories
{
    public interface IGradesRepository: IGenericRepository<Grade>
    {
        IEnumerable<Grade> GetByStudentId(string studentId);
        IEnumerable<Grade> GetByClassSubjectTeacher(int classSubjectTeacher);
        IEnumerable<Grade> GetBySubjectTeacher(int subjectTeacherId);
        IEnumerable<Grade> GetBySubject(int subjectId);
        IEnumerable<Grade> GetByParentId(string parentId);
        IEnumerable<Grade> GetByTeacherId(string teacherId);
        IEnumerable<Grade> GetByTeacherIdStudentId(string teacherId, string studentId);
        IEnumerable<Grade> GetGradesByStudentSubject(string studentId, int subjectId);
        IEnumerable<Grade> GetGradesByStudentClassSubjTeacher(string studentId, int classSubjectTeacherid);
        IEnumerable<Grade> GetByStudentSemester(string studentId, Semester semester);
        IEnumerable<Grade> GetGradesByTeacherClass(string teacherId, int classId);
        IEnumerable<Grade> getByClass(int id);
        IEnumerable<Grade> GetGradesByDate(DateTime startDate, DateTime endDate);
        IEnumerable<Grade> getGradesByDateTeacher(DateTime startDate, DateTime endDate, string userId);
        IEnumerable<Grade> getByStartName(string startName);
        Grade GetFinalSemesterSubjectStudent(Semester FIRST, int subjectId, string studentId);
        IEnumerable<Grade> getByTeacherStudentSubject(string userId, string studentId, int subjectId);
    }
}
