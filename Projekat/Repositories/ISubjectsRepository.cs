using Projekat.Models;
using System.Collections.Generic;

namespace Projekat.Repository
{
    public interface ISubjectsRepository:IGenericRepository<Subject>
    {

        Subject GetByClassAndYear(int ?year, string name);
        //IEnumerable<Subject> GetByTeacherId(string teacherId);

    }
}