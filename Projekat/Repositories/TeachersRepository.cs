using Projekat.Models;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projekat.Repositories
{
    public class TeachersRepository: GenericRepository<Teacher>, ITeachersRepository
    {
        public TeachersRepository(DbContext context) : base(context)

        {

        }

      
       
    }
}