using System;
using System.Linq;
using FluentAssertions;
using WebServiceCaller;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.SharedKernel.Infrastructure.HolidayWebApi
{
    public class HolidayByYearWebServiceGatewayTests
    {
        [Fact]
        public void ShouldNotReturnValuesGiveInvalidExchangeCode()
        {
            var holidayByYearWebServiceGateway = new ExchangeHolidayByYearWebServiceGateway();

            var holidays = holidayByYearWebServiceGateway.GetHolidays("XXXX", 2010, 2014);

            holidays.Length.Should().Be(0);
        }


        [Theory]
        [InlineData("ARCD", 2016)]
        [InlineData("MLVX", 2017)]
        [InlineData("CAVE", 2018)]
        public void ShouldReturnValidHolidaysForSingleYear(
            string exchangeCode,
            int year)
        {
            var holidayByYearWebServiceGateway = new ExchangeHolidayByYearWebServiceGateway();

            var holidays = holidayByYearWebServiceGateway.GetHolidays(exchangeCode, year, year);

            AssertCorrectHolidaysForYear(year, holidays);
        }

        [Theory]
        [InlineData("IMCG", 2010, 2018)]
        [InlineData("DEAL", 2011, 2017)]
        [InlineData("IMCC", 2012, 2016)]
        public void ShouldReturnValidHolidaysForManyYears(
            string exchangeCode,
            int startYear,
            int endYear)
        {
            var holidayByYearWebServiceGateway = new ExchangeHolidayByYearWebServiceGateway();

            var holidays = holidayByYearWebServiceGateway.GetHolidays(exchangeCode, startYear, endYear);

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