using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Abstractions.Repositories;
using VacationRental.Domain.Entities;

namespace VacationRental.Repositories
{
    public class RentalInMemoryRepository : IRentalRepository
    {
        private List<Rental> _collection;
        public RentalInMemoryRepository()
            => _collection = new List<Rental>();

        private int GetIdForNewEntity() => _collection.Count() + 1;
        public Rental Add(Rental entityToAdd)
        {
            entityToAdd.Id = GetIdForNewEntity();
            _collection.Add(entityToAdd);

            return _collection.Where(r => r.Id == entityToAdd.Id).First(); //Not needed, but just be more realistic and return the entity from the collection.
        }

        public IEnumerable<Rental> GetAll()
            => _collection;

        public Rental GetById(int rentalId)
            => _collection.Where(r => r.Id == rentalId).FirstOrDefault();
    }
}
