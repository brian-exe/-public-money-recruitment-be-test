using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Abstractions.Repositories;
using VacationRental.Abstractions.Services;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Exceptions;
using VacationRental.Models;

namespace VacationRental.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalService _rentalService;

        public BookingService(IBookingRepository bookingRepository, IRentalService rentalService)
        {
            _bookingRepository = bookingRepository;
            _rentalService = rentalService;
        }

        private int GetOverlappingsFor(Rental rental, DateTime start, int nights)
        {
            var overlappingsFound = GetBookingsForDatesAndRental(rental.Id, start, start.AddDays(nights + rental.PreparationTimeInDays)).Count();

            return overlappingsFound;
        }

        public void ValidateBookingFor(Rental rental, DateTime start, int nights)
        {
            if (nights <= 0)
                throw new ApplicationException("Nights must be positive");


            var overlappings = GetOverlappingsFor(rental, start, nights);
            if (overlappings >= rental.Units)
                throw new ApplicationException("Not available for booking");
        }

        public Booking AddBooking(BookingBindingModel model)
        {
            var rental = _rentalService.GetRentalById(model.RentalId);

            ValidateBookingFor(rental, model.Start, model.Nights);

            var addedBooking = _bookingRepository.Add(new Booking
            {
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                PreparationTimeInDays = rental.PreparationTimeInDays,
                Unit = GetOverlappingsFor(rental, model.Start, model.Nights) + 1
            });

            return addedBooking;
        }

        public void UpdateBookingsForRental(Rental updatedRental)
        {
            var currentRental = _rentalService.GetRentalById(updatedRental.Id);

            var bookings = GetBookingsForRental(currentRental.Id);
            if (bookings.Count() > 0)
            {
                CheckRentalUpdate(bookings, currentRental, updatedRental);
                UpdateRentalBookingsWith(bookings, updatedRental);
            }
        }

        private void UpdateRentalBookingsWith(IEnumerable<Booking> bookings, Rental updateModel)
        {
            foreach (var booking in bookings)
            {
                booking.PreparationTimeInDays = updateModel.PreparationTimeInDays;
                _UpdateBooking(booking);
            }
        }

        private void _UpdateBooking(Booking booking)
            => _bookingRepository.Update(booking);

        private void CheckRentalUpdate(IEnumerable<Booking> bookings, Rental rental, Rental updateModel)
        {
            CheckPreparationTimeUpdate(bookings, rental, updateModel);
            CheckUnitUpdate(bookings, rental, updateModel);
        }

        private void CheckUnitUpdate(IEnumerable<Booking> bookings, Rental rental, Rental updateModel)
        {
            //if Units is higher or stay the same, no booking adjustment is needed
            if (updateModel.Units >= rental.Units)
                return;

            var newunitsValue = updateModel.Units;

            //Because number of Unit is currently being used as Id, if any booking is assigned to the higher Unit value of the rental, it means that no other Unit was available.
            //At the moment no need for a different solution.

            if (bookings.Any(b => b.Unit == rental.Units))
                throw new ApplicationException("New Unit value conflicts with existing bookings");

        }

        private void CheckPreparationTimeUpdate(IEnumerable<Booking> bookings, Rental rental, Rental updateModel)
        {
            //if PreparationTime decreased or stay the same, no booking adjustment is needed
            if (updateModel.PreparationTimeInDays <= rental.PreparationTimeInDays)
                return;

            var newPreparationTimeInDays = updateModel.PreparationTimeInDays;

            foreach (var booking in bookings)
            {
                if (bookings.Any(b => b.IsDateReserved(booking.End.AddDays(newPreparationTimeInDays))))
                    throw new ApplicationException("Preparation Days update conflicts with existing bookings");
            }
        }


        public IEnumerable<Booking> GetAll() => _bookingRepository.GetAll();

        public IEnumerable<Booking> GetBookingsForDatesAndRental(int rentalId, DateTime start, DateTime end)
        {
            var query = _bookingRepository
                            .AsQueryable()
                                .Where(b => b.RentalId == rentalId && b.IntersectsWithRange(start, end));
            return _bookingRepository.GetFiltered(query);
        }

        public IEnumerable<Booking> GetBookingsForRental(int rentalId)
        {
            var query = _bookingRepository.AsQueryable().Where(b => b.RentalId == rentalId);
            return _bookingRepository.GetFiltered(query);
        }

        public Booking GetBookingById(int bookingId)
        {
            var rental = _bookingRepository.GetById(bookingId);
            if (rental == null)
                throw new EntityNotFoundException("Booking", bookingId.ToString());

            return rental;
        }
    }
}
