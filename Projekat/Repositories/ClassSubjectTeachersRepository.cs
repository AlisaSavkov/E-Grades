using Projekat.Models;
using Projekat.Repositories;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projekat.Repositories
{
    public class ClassSubjectTeachersRepository : GenericRepository<ClassSubjectTeacher>, IClassSubjectTeacherRepository
    {
        public ClassSubjectTeachersRepository(DbContext context) : base(context)
        {

        }

        public ClassSubjectTeacher GetByClassSubjectTeacher(int classId, int subjectTeacherId)
        {
            return Get(x => x.Class.ID == classId && x.SubjectTeacher.ID == subjectTeacherId).FirstOrDefault();
        }

        public IEnumerable<ClassSubjectTeacher> GetBySubject(int subjectId)
        {
            return Get().Where(x => x.SubjectTeacher.Subject.ID == subjectId);
        }

        public IEnumerable<ClassSubjectTeacher> GetByTeacher(string teacherId)
        {
            return  Get().Where(x => x.SubjectTeacher.Teacher.Id == teacherId);
            
        }
        public IEnumerable<ClassSubjectTeacher> GetByClass(int classId)
        {
            IEnumerable<ClassSubjectTeacher> csts = Get().Where(x => x.Class.ID== classId);
            return csts;
        }

        public IEnumerable<ClassSubjectTeacher> GetBySubjectTeacher(int subjectTeacherId)
        {
            return Get().Where(x => x.SubjectTeacher.ID == subjectTeacherId);
        }
        public IEnumerable<ClassSubjectTeacher> GetByClassTeacher(int classId, string teacherId)
        {
            return Get().Where(x => x.SubjectTeacher.Teacher.Id == teacherId && x.Class.ID == classId);
        }

        public IEnumerable<ClassSubjectTeacher> GetByTeacherSubject(string teaherId, int subjectId)
        {
            return Get().Where(x => x.SubjectTeacher.Subject.ID == subjectId && x.SubjectTeacher.Teacher.Id == teaherId);
        }

        public ClassSubjectTeacher GetByCST(int classId, int subjectId, string teacherId)
        {
            return Get(x => x.Class.ID == classId && x.SubjectTeacher.Subject.ID== subjectId && x.SubjectTeacher.Teacher.Id == teacherId).FirstOrDefault();
        }
    }
}