using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacationRental.Abstractions.Services;
using VacationRental.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IUpdateRentalService _updateRentalService;
        private readonly IMapper _mapper;

        public RentalsController(IRentalService rentalService, IUpdateRentalService updateRentalService, IMapper mapper)
        { 
            _rentalService = rentalService;
            _updateRentalService = updateRentalService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        [SwaggerOperation(Summary = "Get Rental", Description = "Get Rental by specified Id")]
        public ActionResult<RentalViewModel> Get(int rentalId)
        {
            var rental = _rentalService.GetRentalById(rentalId);
            return Ok(_mapper.Map<RentalViewModel>(rental));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create Rental", Description = "Create Rental")]
        public ActionResult<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            var addedRental = _rentalService.AddRental(model);
            return Ok(_mapper.Map<ResourceIdViewModel>(addedRental));
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        [SwaggerOperation(Summary = "Update Rental", Description = "Update Rental")]
        public ActionResult<ResourceIdViewModel> Update(int rentalId, RentalBindingModel model)
        {
            var updatedRental = _updateRentalService.UpdateRental(rentalId, model);
            return Ok(_mapper.Map<ResourceIdViewModel>(updatedRental));
        }
    }
}
