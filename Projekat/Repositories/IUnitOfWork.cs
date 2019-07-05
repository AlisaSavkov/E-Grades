
using Projekat.Models;
using Projekat.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Projekat.Repository
{
    public interface IUnitOfWork: IDisposable
    {
       
        IGenericRepository<Parent> ParentsRepository { get; }
        
        IGenericRepository<Admin> AdminsRepository { get; }
        
        IGenericRepository<ApplicationUser> UsersRepository { get; }

        IAuthRepository AuthRepository { get; }
        IClassesRepository ClassesRepository { get; }
        ISubjectsRepository SubjectsRepository { get; }
        ISubjectTeachersRepository SubjectTeachersRepository { get; }
        IStudentsRepository StudentsRepository { get; }
        ITeachersRepository TeachersRepository { get; }
        
        IClassSubjectTeacherRepository ClassSubjectTeachersRepository { get; }
        IGradesRepository GradesRepository { get; }
        void Save();
    }
}
