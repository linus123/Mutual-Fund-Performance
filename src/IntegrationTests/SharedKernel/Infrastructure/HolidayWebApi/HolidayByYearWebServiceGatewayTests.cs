using System;
using System.Linq;
using FluentAssertions;
using MutualFundPerformance.SharedKernel.Infrastructure.HolidayWebApi;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.SharedKernel.Infrastructure.HolidayWebApi
{
    public class HolidayByYearWebServiceGatewayTests
    {
        [Theory]
        [InlineData(2016)]
        [InlineData(2017)]
        [InlineData(2018)]
        public void ShouldReturnValidHolidaysForSingleYear(
            int year)
        {
            var holidayByYearWebServiceGateway = new HolidayByYearWebServiceGateway();

            var holidays = holidayByYearWebServiceGateway.GetHolidays(year, year);

            AssertCorrectHolidaysForYear(year, holidays);
        }

        [Theory]
        [InlineData(2010, 2018)]
        [InlineData(2011, 2017)]
        [InlineData(2012, 2016)]
        public void ShouldReturnValidHolidaysForManyYears(
            int startYear,
            int endYear)
        {
            var holidayByYearWebServiceGateway = new HolidayByYearWebServiceGateway();

            var holidays = holidayByYearWebServiceGateway.GetHolidays(startYear, endYear);

            for (int yearCounter = startYear; yearCounter <= endYear; yearCounter++)
            {
                var holidaysForSingleYear = holidays
                    .Where(h => h.Year == yearCounter)
                    .ToArray();

                AssertCorrectHolidaysForYear(yearCounter, holidaysForSingleYear);
            }
        }

        private static void AssertCorrectHolidaysForYear(
            int expectedYear,
            DateTime[] holidays)
        {
            holidays.Length.Should().BeGreaterOrEqualTo(5);
            holidays.Length.Should().BeLessOrEqualTo(7);

            foreach (var holiday in holidays)
            {
                AssertDateIsNotWeekend(holiday);

                holiday.Year.Should().Be(expectedYear);
            }
        }

        private static void AssertDateIsNotWeekend(DateTime d)
        {
            d.DayOfWeek.Should().NotBe(DayOfWeek.Saturday);
            d.DayOfWeek.Should().NotBe(DayOfWeek.Sunday);
        }
    }
}