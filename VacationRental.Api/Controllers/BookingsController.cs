using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using Swashbuckle.AspNetCore.Annotations;
using VacationRental.Abstractions.Services;
using AutoMapper;
using VacationRental.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IRentalService _rentalService;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingsController(IDictionary<int, BookingViewModel> bookings, IRentalService rentalService, IBookingService bookingService, IMapper mapper)
        {
            _bookings = bookings;
            _rentalService = rentalService;
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a booking", Description = "Get a booking by id")]
        [Route("{bookingId:int}")]
        public ActionResult<BookingViewModel> Get(int bookingId)
        {
            var booking = _bookingService.GetBookingById(bookingId);
            return Ok(_mapper.Map<BookingViewModel>(booking));
        }

        [HttpPost]
        public ActionResult<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            var booking =_bookingService.AddBooking(model);

            return Ok(_mapper.Map<ResourceIdViewModel>(booking));
        }
    }
}
