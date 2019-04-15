using Projekat.Models;
using Projekat.Models.DTOs;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface ISubjectTeacherService
    {
        IEnumerable<SubjectTeacherDTO> GetAllSubjectTeachers();
        //IEnumerable<SubjectTeacherDTO> GetAllSubjectTeachersByTeacher(string Id);
        SubjectTeacherDTO GetDtoById(int Id);
        SubjectTeacher GetById(int Id);
        SubjectTeacher GetBySubjectAndTeacher(int subjectId, string teacherId);
        SubjectTeacherDTO Create(string id, int idSubject);
        SubjectTeacherDTO Update(int Id, int idSubject, string idTeacher);
       // SubjectTeacherDTO RemoveSubjectFromTecher(string id, int isSubject);

        SubjectTeacherDTO Delete(int id);
        IEnumerable<SubjectTeacher> GetAllSubjectTeacher();
        IEnumerable<SubjectTeacher> GetByTeacher(string teacherId);
        IEnumerable<SubjectTeacherDTO> GetByClass(int id);
        SubjectTeacherDTO RemoveSubjectFromTeacher(string id, int subjectId);

        //IEnumerable<SubjectTeacherDTO> GetTeachersBySubject(int subjectId);
    }
}
