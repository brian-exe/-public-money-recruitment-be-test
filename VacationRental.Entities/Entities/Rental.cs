using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Domain.Entities
{
    public class Rental : IEntity
    {
        public int Id { get; set; }
        public int Units { get; set; }
    }
}
