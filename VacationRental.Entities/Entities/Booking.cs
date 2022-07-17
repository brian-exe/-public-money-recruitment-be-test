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
        public int Unit { get; set; }
        public DateTime End { get => Start.Date.AddDays(Nights); }
        public int PreparationTimeInDays { get; set; }

        /// <summary>
        /// Check if the dateToCheck is Booked. PreparationTime is not taken into account.
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <returns>Returns a boolean value corresponding to whether the dateToCheck param is Booked</returns>
        public bool IsDateBooked(DateTime date)
            => date >= Start && date< Start.AddDays(Nights);

        /// <summary>
        /// Check if the date param is reserved by this booking, whether is booked or taken as preparation day.
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <returns>Boolean value indicating if date is reserved</returns>
        public bool IsDateReserved(DateTime dateToCheck)
            => dateToCheck >= Start && dateToCheck < Start.AddDays(Nights + PreparationTimeInDays);

        public bool IntersectsWithRange(DateTime start, DateTime end) 
            => (Start <= end) && (End >= start);


    }
}
