using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Abstractions.Services;
using VacationRental.Api.Models;
using VacationRental.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IMapper _mapper;

        public RentalsController(IRentalService rentalService, IMapper mapper)
        { 
            _rentalService = rentalService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public ActionResult<RentalViewModel> Get(int rentalId)
        {
            var rental = _rentalService.GetRentalById(rentalId);
            return Ok(_mapper.Map<RentalViewModel>(rental));
        }

        [HttpPost]
        public ActionResult<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            var addedRental = _rentalService.AddRental(model);
            return Ok(_mapper.Map<ResourceIdViewModel>(addedRental));
        }
    }
}
