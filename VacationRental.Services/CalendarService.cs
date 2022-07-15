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
            for (var i = 0; i < model.Nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = model.Start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                foreach (var booking in _bookingService.GetAll())
                {
                    if (booking.RentalId == rental.Id
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
