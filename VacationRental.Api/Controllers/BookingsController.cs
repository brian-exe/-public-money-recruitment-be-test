using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingsController(IBookingService bookingService, IMapper mapper)
        {
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
        [SwaggerOperation(Summary = "Create Booking", Description = "Create a new Booking based on payload")]
        public ActionResult<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            var booking =_bookingService.AddBooking(model);

            return Ok(_mapper.Map<ResourceIdViewModel>(booking));
        }
    }
}
