using Projekat.Models;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projekat.Repositories
{
    public class SubjectTeachersRepository : GenericRepository<SubjectTeacher>, ISubjectTeachersRepository
    {
        public SubjectTeachersRepository(DbContext context) : base(context)

        {
        }


        public IEnumerable<SubjectTeacher> GetBySubjectId(int id)
        {
            return Get().Where(x => x.Subject.ID == id);
        }

        public SubjectTeacher GetBySubjectTeacher(int subjectId, string teacherId)
        {
            return Get(x => x.Teacher.Id == teacherId && x.Subject.ID == subjectId).FirstOrDefault();
        }

        public IEnumerable<SubjectTeacher> GetByTeacherId(string id)
        {
            return Get().Where(x => x.Teacher.Id == id);
        }

        public IEnumerable<SubjectTeacher> GetNoByTeacherId(string teacherId)
        {
            return Get().Where(x => x.Teacher.Id != teacherId);
        }
    }
}