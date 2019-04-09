using Projekat.Models;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Repositories
{
    public interface ISubjectTeachersRepository: IGenericRepository<SubjectTeacher>
    {
        IEnumerable<SubjectTeacher> GetByTeacherId(string id);
        //IEnumerable<SubjectTeacher> GetByStudentId(string id);
        IEnumerable<SubjectTeacher> GetBySubjectId(int id);
        SubjectTeacher GetBySubjectTeacher(int subjectId, string teacherId);
        IEnumerable<SubjectTeacher> GetNoByTeacherId(string teacherId);
    }
}
