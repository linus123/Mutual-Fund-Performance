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

            var result = holidayByYearWebServiceGateway.GetHolidays("XXXX", 2010, 2014);

            result.ExchangeIsoCode.Should().Be("XXXX");
            result.StartYear.Should().Be(2010);
            result.EndYear.Should().Be(2014);
            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Exchange was not found.");
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

            var result = holidayByYearWebServiceGateway.GetHolidays(exchangeCode, year, year);

            result.ExchangeIsoCode.Should().Be(exchangeCode);
            result.StartYear.Should().Be(year);
            result.EndYear.Should().Be(year);
            result.HasError.Should().BeFalse();
            result.ErrorMessage.Should().BeNullOrEmpty();

            AssertCorrectHolidaysForYear(year, result.Holidays);
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

            var result = holidayByYearWebServiceGateway.GetHolidays(exchangeCode, startYear, endYear);

            result.ExchangeIsoCode.Should().Be(exchangeCode);
            result.StartYear.Should().Be(startYear);
            result.EndYear.Should().Be(endYear);
            result.HasError.Should().BeFalse();
            result.ErrorMessage.Should().BeNullOrEmpty();

            for (int yearCounter = startYear; yearCounter <= endYear; yearCounter++)
            {
                var holidaysForSingleYear = result.Holidays
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