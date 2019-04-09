using Projekat.Models;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface IStudentService
    {
        IEnumerable<StudentDTO> GetAllStudentsDTOs();
        IEnumerable<Student> GetAllStudents();
        //IEnumerable<StudentDTO> GetDTOByClassId(int classId);
        IEnumerable<Student> GetByClassId(int classId);
        StudentDTO GetDtoById(string id);
        Student GetById(string id);
        StudentDTO AddImageToStudent(string id, string fileName);
        StudentDTO AddStudentToClass(string idStudent, int idClass);
        //dodati remove student from class i get students by class
        IEnumerable<StudentDTO> GetByParentId(string parentId);
        IEnumerable<StudentPDTO> GetByParentIdP(string parentId);
        StudentDTO Update(string id, StudentDTO dto);

        StudentDTO Delete(string id);
        Student GetByUsername(string username);
        IEnumerable<StudentPDTO> GetByClassIdTeacherIdSubjectId(int classId, string teacherId, int subjectId);
    }
}
