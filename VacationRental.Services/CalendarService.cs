using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Abstractions.Services;
using VacationRental.Models;

namespace VacationRental.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IRentalService _rentalService;
        private readonly IBookingService _bookingService;

        public CalendarService(IRentalService rentalService, IBookingService bookingService)
        {
            _rentalService = rentalService;
            _bookingService = bookingService;
        }
        public CalendarViewModel GetCalendar(GetCalendarModel model)
        {

            if (model.Nights < 0)
                throw new ApplicationException("Nights must be positive");

            var rental = _rentalService.GetRentalById(model.RentalId);

            var result = new CalendarViewModel
            {
                RentalId = rental.Id,
                Dates = new List<CalendarDateViewModel>()
            };

            var bookedDates = _bookingService.GetBookingsForDatesAndRental(rental.Id, model.Start, model.Start.Date.AddDays(model.Nights));

            for (var i = 0; i < model.Nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = model.Start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                var bookingsForDate = bookedDates.Where(b => b.IsDateBooked(date.Date));

                foreach (var booking in bookingsForDate)
                {
                    date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
