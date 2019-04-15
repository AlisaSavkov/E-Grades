
using Projekat.Models;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface IClassSubjectTeacherService
    {
        IEnumerable<ClassSubjectTeacherDTO> GetAllDTOs();
        //IEnumerable<SubjectTeacherDTO> GetAllSubjectTeachersByTeacher(string Id);
        ClassSubjectTeacher GetById(int Id);
        ClassSubjectTeacherDTO GetDtoById(int Id);
        IEnumerable<ClassSubjectTeacher> GetAll();
        IEnumerable<ClassSubjectTeacher> GetByTeacher(string id);
        ClassSubjectTeacherDTO Create(string teacherId, int subjectId, int classId);
        ClassSubjectTeacherDTO Update(int id, string teacherId, int subjectId, int classId);
        IEnumerable<ClassSubjectTeacher> GetByClass(int classId);
        //ClassSubjectTeacherDTO RemoveClassFromSubjectTeacher(string id, int isSubject);
        ClassSubjectTeacher GetByClassSubjectTeacher(int classId, int subjectTeacherId);
        ClassSubjectTeacherDTO Delete(int id);
        IEnumerable<ClassSubjectTeacher> GetByClassTeacher(int classId, string teacherId);
        ClassSubjectTeacherDTO GetByCST(int classId, int subjectId, string teacherId);
        ClassSubjectTeacherDTO Create1(int subTeacher, int classId);
        ClassSubjectTeacherDTO RemoveSubjectFromClass(int classId, int stId);
        //ClassSubjectTeacherDTO Update1(int id, ClassSubjectTeacherUpdateDTO dto);
        //ClassSubjectTeacherDTO Remove(int subjectId, string teacherId, int classId);

        //IEnumerable<SubjectTeacherDTO> GetTeachersBySubject(int subjectId);
    }
}
