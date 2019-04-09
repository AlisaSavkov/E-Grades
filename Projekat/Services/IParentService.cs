using Projekat.Models;
using Projekat.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface IParentService
    {

        //IEnumerable<ParentDTO> GetAllParents();
        IEnumerable<Parent> GetAllParents();
        ParentDTO GetDtoById(string id);
        Parent GetByUserName(string userName);
        //dodati remove student from class i get students by class
        Parent GetById(string id);
        ParentDTO Update(string id, ParentDTO dto);

        ParentDTO Delete(string id);
        Parent GetByJMBG(string jmbg);
    }
}
