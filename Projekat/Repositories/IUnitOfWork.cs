
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
        //IGenericRepository<Year> YearsRepository { get; }
        //IGenericRepository<Class> ClassesRepository { get; }
        //IGenericRepository<Grade> GradesRepository { get; }
        IGenericRepository<Parent> ParentsRepository { get; }
        //IGenericRepository<Student> StudentsRepository { get; }
        //IGenericRepository<Teacher> TeachersRepository { get; }
        IGenericRepository<Admin> AdminsRepository { get; }
        //IGenericRepository <Subject> SubjectsRepository { get; }
        //ISubjectsRepository<Subject> SubjectsRepository { get; }
        //IGenericRepository<SubjectTeacher> SubjectTeachersRepository { get; }
        IGenericRepository<ApplicationUser> UsersRepository { get; }

        IAuthRepository AuthRepository { get; }
        IClassesRepository ClassesRepository { get; }
        ISubjectsRepository SubjectsRepository { get; }
        ISubjectTeachersRepository SubjectTeachersRepository { get; }
        IStudentsRepository StudentsRepository { get; }
        ITeachersRepository TeachersRepository { get; }
        //IGenericRepository<ClassSubjectTeacher> ClassSubjectTeachersRepository { get; }
        IClassSubjectTeacherRepository ClassSubjectTeachersRepository { get; }
        IGradesRepository GradesRepository { get; }
        void Save();
    }
}
