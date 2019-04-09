using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity GetByID(object id);

        void Insert(TEntity entity);

        void Delete(object id);

        IEnumerable<TEntity> Get(
           //funkcija koja prima entity i izlaz je bool, prosledjujemo kao filter funk koje ce da zadovolje te uslove,
           //ulazni parametar je klasa za koju pravimo
           //repository i ako zadovoljava uslov bice povucen iz baze, ako ne nece biti u kolekciji koju dobijamo
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "");

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}
