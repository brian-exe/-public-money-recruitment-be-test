using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Abstractions.Repositories;
using VacationRental.Abstractions.Services;
using VacationRental.Domain.Entities;
using VacationRental.Models;

namespace VacationRental.Services
{
    public class UpdateRentalService : IUpdateRentalService
    {
        private readonly IRentalService _rentalService;
        private readonly IBookingService _bookingService;
        private readonly IRentalRepository _rentalRepository;

        public UpdateRentalService(IRentalService rentalService, IBookingService bookingService, IRentalRepository rentalRepository)
        {
            _rentalService = rentalService;
            _bookingService = bookingService;
            _rentalRepository = rentalRepository;
        }

        public Rental UpdateRental(int rentalId, RentalBindingModel updateModel)
        {
            var rentalForUpdate = _rentalService.GetRentalById(rentalId);
            _bookingService.UpdateBookingsForRental(new Rental()
            {
                Id = rentalId,
                PreparationTimeInDays = updateModel.PreparationTimeInDays,
                Units = updateModel.Units
            });

            rentalForUpdate.PreparationTimeInDays = updateModel.PreparationTimeInDays;
            rentalForUpdate.Units = updateModel.Units;

            return _UpdateRental(rentalForUpdate);
        }

        private Rental _UpdateRental(Rental existingRental)
            => _rentalRepository.Update(existingRental);
    }
}
