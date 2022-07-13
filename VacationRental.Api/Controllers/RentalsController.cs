using System;
using System.Collections.Generic;
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

        public RentalsController(IRentalService rentalService)
            => _rentalService = rentalService;

        [HttpGet]
        [Route("{rentalId:int}")]
        public ActionResult<RentalViewModel> Get(int rentalId)
        {
            return Ok(_rentalService.GetRentalById(rentalId));
            //if (!_rentals.ContainsKey(rentalId))
            //    throw new ApplicationException("Rental not found");

            //return _rentals[rentalId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var addedRental = _rentalService.AddRental(model);
            var key = new ResourceIdViewModel { Id = addedRental.Id};

            //_rentals.Add(key.Id, new RentalViewModel
            //{
            //    Id = key.Id,
            //    Units = model.Units
            //});

            return key;
        }
    }
}
