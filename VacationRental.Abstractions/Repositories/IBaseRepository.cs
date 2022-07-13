using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationRental.Abstractions.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity: class
    {
        TEntity GetById(int entityId);
        IEnumerable<TEntity> GetAll();
        TEntity Add(TEntity entityToAdd);
    }
}
