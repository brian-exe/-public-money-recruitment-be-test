using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Abstractions.Repositories;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Repositories
{
    public abstract class BaseInMemoryRepository<TEntity> : IBaseRepository<TEntity> where TEntity : IEntity
    {
        private List<TEntity> _collection;

        public BaseInMemoryRepository()
            => _collection = new List<TEntity>();

        private int GetIdForNewEntity() => _collection.Count() + 1;

        public TEntity Add(TEntity entityToAdd)
        {
            entityToAdd.Id = GetIdForNewEntity();
            _collection.Add(entityToAdd);

            return _collection.Where(r => r.Id == entityToAdd.Id).First(); //Not really needed, but just be more realistic and return the entity from the collection.
        }

        public IQueryable<TEntity> AsQueryable()
            => _collection.AsQueryable();

        public IEnumerable<TEntity> GetAll()
            => _collection;

        public IEnumerable<TEntity> GetFiltered(IQueryable<TEntity> filter)
            => filter.ToList();

        public TEntity GetById(int entityId)
            => _collection.Where(r => r.Id == entityId).FirstOrDefault();
    }
}
