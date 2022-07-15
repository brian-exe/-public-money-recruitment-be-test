using System;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Domain.Entities
{
    public class Booking : IEntity
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public DateTime End() => Start.Date.AddDays(Nights);

        public bool IsDateBooked(DateTime dateToCheck)
            => dateToCheck >= Start && dateToCheck <= Start.AddDays(Nights);

        public bool IntersectsWithRange(DateTime start, DateTime end) => (Start <= end) && (End() >= start);


    }
}
