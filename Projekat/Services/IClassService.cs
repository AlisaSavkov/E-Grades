using Projekat.Models.DTOs;
using Projekat.Models;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface IClassService
    {
        IEnumerable<ClassDTO> GetAllClasses();
        IEnumerable<ClassDTO> GetAllClassesByTeacher(string teacherId);
        Class GetById(int id);
        ClassDTO GetDtoById(int id);
        ClassDTO Create(ClassDTO clas);
        //ClassDTO Update(int id, ClassUpdateDTO dto);

        ClassDTO Delete(int id);
        ClassDTO Update(int id, ClassDTO dto);
        IEnumerable<ClassDTO> GetAllClassesByTeacherSubject(string teacherId, int subjectId);
    }
}
