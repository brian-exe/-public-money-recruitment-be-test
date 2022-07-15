using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Models;

namespace VacationRental.Abstractions.Services
{
    public interface IBookingService
    {
        Booking GetBookingById(int bookingId);
        Booking AddBooking(BookingBindingModel rentalBindingModel);
        IEnumerable<Booking> GetAll();
    }
}
