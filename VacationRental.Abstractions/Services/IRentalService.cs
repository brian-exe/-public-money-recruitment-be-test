using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Models;

namespace VacationRental.Abstractions.Services
{
    public interface IRentalService
    {
        Rental GetRentalById(int rentalId);
        Rental AddRental(RentalBindingModel rentalBindingModel);

    }
}
