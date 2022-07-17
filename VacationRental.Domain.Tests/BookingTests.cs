using System;
using VacationRental.Domain.Entities;
using Xunit;

namespace VacationRental.Domain.Tests
{
    public class BookingTest
    {
        private Booking bookingForTest;
        public BookingTest()
        {
            bookingForTest = new Booking()
            {
                Id = 0,
                Nights = 5,
                RentalId = 0,
                Start = DateTime.Now
            };
        }
        [Fact]
        public void IsDateBooked_WhenDateIsBooked_ReturnsTrue()
        {
            //arrange
            var dateForCheck = bookingForTest.Start.AddDays(1);

            //act
            var result = bookingForTest.IsDateBooked(dateForCheck);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void IsDateBooked_WhenDateIsNotBooked_ReturnsFalse()
        {
            //arrange
            var dateForCheck = bookingForTest.Start.AddDays(6);

            //act
            var result = bookingForTest.IsDateBooked(dateForCheck);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void IntersectsWithRange_WhenCalledWithSameDates_ReturnsTrue()
        {
            //arrange
            var start = bookingForTest.Start;
            var end = bookingForTest.End;

            //same dates
            var result = bookingForTest.IntersectsWithRange(start,end);
            Assert.True(result);
        }
        
        [Fact]
        public void IntersectsWithRange_WhenBothDatesAreContained_ReturnsTrue()
        {
            //arrange
            var start = bookingForTest.Start.AddDays(1);
            var end = bookingForTest.End.AddDays(-1);

            //same dates
            var result = bookingForTest.IntersectsWithRange(start,end);
            Assert.True(result);
        }
        
        [Fact]
        public void IntersectsWithRange_WhenStartDateIsContained_ReturnsTrue()
        {
            //arrange
            var start = bookingForTest.Start.AddDays(1);
            var end = bookingForTest.End.AddDays(1);

            //same dates
            var result = bookingForTest.IntersectsWithRange(start, end);
            Assert.True(result);
        }
        
        [Fact]
        public void IntersectsWithRange_WhenOnlyEndDateIsContained_ReturnsTrue()
        {
            //arrange
            var start = bookingForTest.Start.AddDays(-10);
            var end = bookingForTest.End.AddDays(-1);

            //same dates
            var result = bookingForTest.IntersectsWithRange(start, end);
            Assert.True(result);
        }

        [Fact]
        public void IsDateReserved_WhenDateIsReserved_ReturnsTrue()
        {
            //arrange
            bookingForTest.PreparationTimeInDays = 1;
            var dateForCheck = bookingForTest.End.AddDays(1);

            //act
            var result = bookingForTest.IsDateReserved(dateForCheck);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void IsDateReserved_WhenDateIsNotReserved_ReturnsFalse()
        {
            //arrange
            bookingForTest.PreparationTimeInDays = 1;
            var dateForCheck = bookingForTest.End.AddDays(2);

            //act
            var result = bookingForTest.IsDateReserved(dateForCheck);

            //assert
            Assert.False(result);
        }

    }
}
