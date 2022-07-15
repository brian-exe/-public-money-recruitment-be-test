using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Abstractions.Repositories;
using VacationRental.Abstractions.Services;
using VacationRental.Api.MapperProfiles;
using VacationRental.Api.Middlewares;
using VacationRental.Models;
using VacationRental.Repositories;
using VacationRental.Services;

namespace VacationRental.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(opts =>
                {
                    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" });
                    opts.EnableAnnotations();
               }
            );

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new RentalProfile());
                mc.AddProfile(new BookingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>());
            services.AddTransient<IRentalService, RentalService>();
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddSingleton<IRentalRepository, RentalInMemoryRepository>();
            services.AddSingleton<IBookingRepository, BookingInMemoryRepository>();
            services.AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseRouting();
            app.UseEndpoints(e => e.MapControllers());
        }
    }
}
