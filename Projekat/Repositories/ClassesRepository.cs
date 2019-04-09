using Projekat.Models;
using Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projekat.Repositories
{
    public class ClassesRepository : GenericRepository<Class>, IClassesRepository
    {
        public ClassesRepository(DbContext context) : base(context)
        {
        }

        public Class GetByYearLabel(int? year, string label)
        {
            return Get(x => x.Year == year && x.Label == label).FirstOrDefault();
        }
    }
}