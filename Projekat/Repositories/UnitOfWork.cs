

using Projekat.Repositories;
using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using Unity.Attributes;

namespace Projekat.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private DbContext db;
        public UnitOfWork(DbContext db)
        {
            this.db = db;
        }
       
        [Dependency]
        public IGenericRepository<Parent> ParentsRepository { get; set; }
       
        [Dependency]
        public IGenericRepository<ApplicationUser> UsersRepository { get; set; }
        [Dependency]
        public IAuthRepository AuthRepository { get; set; }
       
        [Dependency]
        public IClassesRepository ClassesRepository { get; set; }
        [Dependency]
        public IGenericRepository<Admin> AdminsRepository { get; set; }
        [Dependency]
        public ISubjectsRepository SubjectsRepository { get; set; }
        [Dependency]
        public IStudentsRepository StudentsRepository { get; set; }
        [Dependency]
        public ITeachersRepository TeachersRepository { get; set; }
        [Dependency]
        public ISubjectTeachersRepository SubjectTeachersRepository { get; set; }
        [Dependency]
        public IClassSubjectTeacherRepository ClassSubjectTeachersRepository { get; set; }
        [Dependency]
        public IGradesRepository GradesRepository { get; set; }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}