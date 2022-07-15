using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Abstractions.Services;
using VacationRental.Domain.Entities;
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

            var bookingsFound = _bookingService.GetBookingsForDatesAndRental(rental.Id, model.Start, model.Start.Date.AddDays(model.Nights));

            for (var i = 0; i < model.Nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = model.Start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                };
                result.Dates.Add(date);
            }

            //TODO: Revisar este for que se puede quitar y pasarlo al de arriba y sumar un metodo en el booking para saber si está ocupado por limpieza
            foreach(Booking b in bookingsFound)
            {
                for(var j =0; j< b.Nights; j++)
                {
                    result.Dates
                        .First(d => d.Date == b.Start.AddDays(j))
                            .Bookings
                                .Add(new CalendarBookingViewModel { Id = b.Id, Unit = b.Unit });
                }

                for (var j = 0; j < rental.PreparationTimeInDays; j++)
                {
                    result.Dates.First(d => d.Date == b.Start.AddDays(j+b.Nights)).PreparationTimes.Add(new CalendarPreparationTimeViewModel {Unit = b.Unit });
                }
            }            

            return result;
        }
    }
}
