using System;
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