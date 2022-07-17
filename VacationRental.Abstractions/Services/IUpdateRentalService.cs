using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Models;

namespace VacationRental.Abstractions.Services
{
    public interface IUpdateRentalService
    {
        Rental UpdateRental(int rentalId, RentalBindingModel rentalBindingModel);
    }
}
