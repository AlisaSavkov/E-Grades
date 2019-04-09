using Projekat.Models.DTOs;
using Projekat.ModelsIzmena.DTOs;
using Projekat.Models;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface ISubjectService
    {
        IEnumerable<SubjectDTO> GetAllSubjects();
        IEnumerable<SubjectDTO> GetSubjectsByTeacher(string teacherId);
        IEnumerable<SubjectDTO> GetSubjectsByStudent(string studentId);
        SubjectDTO GetDtoById(int Id);
        SubjectDTO Create(SubjectDTO dto);
        SubjectDTO Update(int Id, SubjectUpdateDTO dto);

        SubjectDTO Delete(int Id);
        Subject GetById(int subjectId);
        IEnumerable<SubjectDTO> GetSubjectsNoByTeacher(string teacherId);
    }
}
