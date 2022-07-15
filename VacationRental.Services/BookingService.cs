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
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalService _rentalService;

        public BookingService(IBookingRepository bookingRepository, IRentalService rentalService)
        {
            _bookingRepository = bookingRepository;
            _rentalService = rentalService;
        }

        public Booking AddBooking(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nights must be positive");

            var rental = _rentalService.GetRentalById(model.RentalId);
            var query = _bookingRepository
                        .AsQueryable()
                            .Where(b => b.RentalId == rental.Id)
                            .Where(b => b.IntersectsWithRange(model.Start, model.Start.AddDays(model.Nights)));

            var overlappingsFound = _bookingRepository.GetFiltered(query).Count();

            if (overlappingsFound >= rental.Units)
                throw new ApplicationException("Not available for booking");

            var addedBooking = _bookingRepository.Add(new Booking
            {
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });

            return addedBooking;
        }

        public IEnumerable<Booking> GetAll() => _bookingRepository.GetAll();

        public IEnumerable<Booking> GetBookingsForDatesAndRental(int rentalId, DateTime start, DateTime end)
        {
            var query = _bookingRepository
                            .AsQueryable()
                                .Where(b => b.RentalId == rentalId && b.IntersectsWithRange(start, end));
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
