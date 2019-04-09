using Projekat.Models;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Repositories
{
    public interface IStudentsRepository: IGenericRepository<Student>
    {
        IEnumerable<Student> GetByClassId(int classId);
        IEnumerable<Student> GetByParentId(string parentId);
        Student GetByUsername(string username);
    }
}
