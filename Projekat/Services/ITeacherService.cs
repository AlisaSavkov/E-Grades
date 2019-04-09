using Projekat.Models;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface ITeacherService
    {
        IEnumerable<TeacherDTO> GetAllTeachersDTO();
        IEnumerable<Teacher> GetAllTeachers();
        IEnumerable<TeacherDTO> GetTeachersBySubject(int subjectId);
        TeacherDTO GetDtoById(string id);
        Teacher GetById(string id);

        TeacherDTO Update(string id, TeacherDTO dto);

        TeacherDTO Delete(string id);
        
        //SubjectTeacherDTO AddSubjectTeacher(int id, int subjectId);
    }
}
