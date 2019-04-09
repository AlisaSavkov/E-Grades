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
    public class StudentsRepository: GenericRepository<Student>, IStudentsRepository
    {

        public StudentsRepository(DbContext context) : base(context)

        {
           
        }

        public IEnumerable<Student> GetByClassId(int classId)
        {
            return Get().Where(x => x.Class.ID == classId);
        }

        public IEnumerable<Student> GetByParentId(string parentId)
        {
            return Get().Where(x => x.Parent.Id == parentId);
        }

        public Student GetByUsername(string username)
        {
            return Get().Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefault();
        }
    }
}