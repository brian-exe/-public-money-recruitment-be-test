using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Models;

namespace VacationRental.Api.MapperProfiles
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Rental, RentalViewModel>();
            CreateMap<RentalViewModel, Rental>();
            CreateMap<Rental, ResourceIdViewModel>();
        }
    }
}
