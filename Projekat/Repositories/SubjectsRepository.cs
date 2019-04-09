using Projekat.Models;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Projekat.Repositories
{
    public class SubjectsRepository : GenericRepository<Subject>, ISubjectsRepository
    {
        public SubjectsRepository(DbContext context) : base(context)
        {

        }

        public Subject GetByClassAndYear(int ?year, string name)
        {
            return Get(x => x.Year == year && x.Name == name).FirstOrDefault();
        }

        //public IEnumerable<Subject> GetByTeacherId(string teacherId)
        //{
        //    return Get().Where(x => x.SubjectTeachers. == teacherId);
        //}

       
    }
}