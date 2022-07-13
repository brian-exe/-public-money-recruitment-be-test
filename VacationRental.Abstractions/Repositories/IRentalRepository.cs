using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;

namespace VacationRental.Abstractions.Repositories
{
    public interface IRentalRepository : IBaseRepository<Rental>
    {
    }
}
