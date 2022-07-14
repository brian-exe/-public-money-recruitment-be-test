using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Abstractions.Repositories;
using VacationRental.Abstractions.Services;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Exceptions;
using VacationRental.Models;

namespace VacationRental.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalService(IRentalRepository rentalRepository)
            => _rentalRepository = rentalRepository;

        public Rental AddRental(RentalBindingModel rentalBindingModel)
            => _rentalRepository.Add(new Rental() { Units = rentalBindingModel.Units });

        public Rental GetRentalById(int rentalId)
        {
            var rental = _rentalRepository.GetById(rentalId);
            if (rental == null)
                throw new EntityNotFoundException("Rental", rentalId.ToString());

            return rental;
        }
    }
}
