using Projekat.Models;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Repositories
{
    public interface ITeachersRepository : IGenericRepository<Teacher>
    {
        //IEnumerable<Teacher> GetBySubjectId(int subjectId);
    }
}
