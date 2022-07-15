using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Abstractions.Services;
using VacationRental.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
            => _calendarService = calendarService;

        [HttpGet]
        public ActionResult<CalendarViewModel> Get([FromQuery] GetCalendarModel getCalendarModel)
            => Ok(_calendarService.GetCalendar(getCalendarModel));
    }
}
