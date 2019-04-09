using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Services
{
    public interface IAdminService
    {
        IEnumerable<AdminDTO> GetAll();
        AdminDTO GetDtoById(string id);
        AdminDTO GetByUserName(string userName);
        //dodati remove student from class i get students by class
        Admin GetById(string id);
        AdminDTO Update(string id, AdminDTO dto);

        AdminDTO Delete(string id);
    }
}
