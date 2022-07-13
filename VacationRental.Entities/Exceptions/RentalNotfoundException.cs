using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationRental.Domain.Exceptions
{
    public class RentalNotfoundException : NotFoundException
    {
        public RentalNotfoundException(int rentalId) : base ($"Rental with id {rentalId} was not found")
        {}
    }
}
