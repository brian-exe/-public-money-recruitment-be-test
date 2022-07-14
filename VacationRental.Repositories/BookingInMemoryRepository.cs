using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Abstractions.Repositories;
using VacationRental.Domain.Entities;

namespace VacationRental.Repositories
{
    public class BookingInMemoryRepository : BaseInMemoryRepository<Booking>, IBookingRepository
    {    }
}
