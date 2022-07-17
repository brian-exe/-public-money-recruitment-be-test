using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Abstractions.Repositories;
using VacationRental.Domain.Entities;

namespace VacationRental.Repositories
{
    public class BookingInMemoryRepository : BaseInMemoryRepository<Booking>, IBookingRepository
    {
        public Booking Update(Booking bookingUpdated)
        {
            var updated = GetById(bookingUpdated.Id);
            updated.PreparationTimeInDays = bookingUpdated.PreparationTimeInDays;
            return updated;
        }
    }
}
