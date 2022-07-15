using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Models;

namespace VacationRental.Abstractions.Services
{
    public interface ICalendarService
    {
        CalendarViewModel GetCalendar(GetCalendarModel model);
    }
}
