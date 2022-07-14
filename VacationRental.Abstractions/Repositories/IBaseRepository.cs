using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Abstractions.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity: IEntity
    {
        TEntity GetById(int entityId);
        IEnumerable<TEntity> GetAll();
        TEntity Add(TEntity entityToAdd);
        IQueryable<TEntity> AsQueryable();
        IEnumerable<TEntity> GetFiltered(IQueryable<TEntity> filter);
    }
}
