using System;
using System.Linq;
using FluentAssertions;
using MutualFundPerformance.SharedKernel.Infrastructure.HolidayWebApi;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.SharedKernel.Infrastructure.HolidayWebApi
{
    public class HolidayByYearWebServiceGatewayTests
    {
        [Fact]
        public void ShouldReturnValidHolidaysForSingleYear()
        {
            var holidayByYearWebServiceGateway = new HolidayByYearWebServiceGateway();

            var holidays = holidayByYearWebServiceGateway.GetHolidays(2018, 2018);

            AssertCorrectHolidaysForYear(holidays);
        }

        [Fact]
        public void ShouldReturnValidHolidaysForManyYears()
        {
            var holidayByYearWebServiceGateway = new HolidayByYearWebServiceGateway();

            var holidays = holidayByYearWebServiceGateway.GetHolidays(2010, 2018);

            for (int yearCounter = 2010; yearCounter <= 2018; yearCounter++)
            {
                var holidaysForSingleYear = holidays.Where(h => h.Year == yearCounter).ToArray();
                AssertCorrectHolidaysForYear(holidaysForSingleYear);
            }
        }

        private static void AssertCorrectHolidaysForYear(
            DateTime[] holidays)
        {
            holidays.Length.Should().BeGreaterOrEqualTo(5);
            holidays.Length.Should().BeLessOrEqualTo(7);

            foreach (var holiday in holidays)
            {
                holiday.DayOfWeek.Should().NotBe(DayOfWeek.Saturday);
                holiday.DayOfWeek.Should().NotBe(DayOfWeek.Sunday);
            }
        }
    }
}