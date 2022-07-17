using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Models;

namespace VacationRental.Api.MapperProfiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, ResourceIdViewModel>();
            CreateMap<Booking, BookingViewModel>();
            CreateMap<BookingViewModel, Booking>();
        }
    }
}
