using Projekat.Models;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Repositories
{
    public interface IClassSubjectTeacherRepository: IGenericRepository<ClassSubjectTeacher>
    {
        ClassSubjectTeacher GetByClassSubjectTeacher(int classId, int subjectTeacherId);

        IEnumerable<ClassSubjectTeacher> GetBySubjectTeacher(int subjectTeacherId);
        IEnumerable<ClassSubjectTeacher> GetByTeacher(string teacherId);
        IEnumerable<ClassSubjectTeacher> GetByClass(int classId);
        IEnumerable<ClassSubjectTeacher> GetBySubject(int subjectId);
        IEnumerable<ClassSubjectTeacher> GetByClassTeacher(int classId, string teacherId);
        IEnumerable<ClassSubjectTeacher> GetByTeacherSubject(string teaherId, int subjectId);
        ClassSubjectTeacher GetByCST(int classId, int subjectId, string teacherId);
        
    }
}
