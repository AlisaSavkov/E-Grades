
using Projekat.Models;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface IGradeService
    {
        IEnumerable<GradeDTO> GetAllGrades();
        IEnumerable<GradeDTO> GetGradesByStudent(string studentId);
        IEnumerable<GradeDTO> GetGradesBySubject(int subjectId);
        //IEnumerable<Grade> GetGradesBySubject(int subjectId);
        IEnumerable<GradeDTO> GetGradesByTeacher(string teacherId);
        IEnumerable<GradeDTO> GetGradesByTeacherStudent(string teacherId, string studentId);
        IEnumerable<GradeDTO> GetGradesByTeacherSubject(string teacherId, int subjectId);
        IEnumerable<GradeDTO> GetGradesByStudentSemester(string studentId, Semester semester);
        IEnumerable<GradeDTO> GetGradesByStudentSubject(string studentId, int subjectId);
        GradeDTO GetDtoById(int id);
        Grade GetById(int id);
        IEnumerable<GradeDTO> GetGradesByParent(string parentId);
        GradeDTO Update(int id, GradeUpdateDTO gradeDto);
        GradeDTO Create(string studentId, string teacherId, int subjectId, GradeCreateDTO dto);
        GradeDTO Delete(int id);
        IEnumerable<GradeDTO> getByClass(int id);
        IEnumerable<GradeDTO> GetGradesByTeacherClass(string id, int iD);
        IEnumerable<GradeDTO> GetGradesByDate(DateTime startDate, DateTime endDate);
        IEnumerable<GradeDTO> GetGradesByDateTeacher(DateTime startDate, DateTime endDate, string userId);
        IEnumerable<GradeDTO> GetByStartName(string text);
        IEnumerable<GradeDTO> GetByStartNameTeacherId(string startName, string id);
       
        IEnumerable<GradeDTO> GetGradesByStudentClassSubjTeacher(string studentId, int classSubjectTeacherid);
        IEnumerable<GradeDTO> GetGradesByTeacherStudentSemester(string teacherId, string studentId, Semester semester);
        IEnumerable<GradeDTO> GetGradesByTeacherStudentSubject(string userId, string studentId, int subjectId);
    }
}
