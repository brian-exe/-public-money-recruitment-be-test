using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task UpdateRental_WhenUpdatingWithNoExistingBooking_RentalIsUpdated()
        {
            var request = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            request.Units = 3;
            using (var postResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
            
            request.PreparationTimeInDays = 2;
            using (var postResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task UpdateRental_WhenBookingsExistButNoConflict_RentalIsUpdated()
        {
            var request = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postRentalResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            postBooking1Request.Start = new DateTime(2002, 01, 10);

            ResourceIdViewModel postBooking2Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            request.PreparationTimeInDays = 2;
            request.Units = 1;
            using (var postResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postRentalResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }

        }

        [Fact]
        public async Task UpdateRental_WhenUpdatingUnitsForRentalWithBookingsOccupyingAllUnits_ThenRequestReturnsBadRequest()
        {
            var request = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }


            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            postBooking1Request = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            request.Units = 1;
            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", request))
            {
                Assert.False(putResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
            }

        }

        [Fact]
        public async Task GivenCompleteRequest_WhenUpdatingPreparationDaysForRentalWithBookings_ThenRequestReturnsBadRequest()
        {
            var request = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }


            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            postBooking1Request = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 03)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            request.PreparationTimeInDays = 2;
            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", request))
            {
                Assert.False(putResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
            }

        }
    }
}
