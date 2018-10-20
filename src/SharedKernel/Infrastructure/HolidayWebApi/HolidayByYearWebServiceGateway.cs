using System;
using System.Collections.Generic;

namespace MutualFundPerformance.SharedKernel.Infrastructure.HolidayWebApi
{
    public class HolidayByYearWebServiceGateway
    {
        private readonly Random _random;

        public HolidayByYearWebServiceGateway()
        {
            _random = new Random();
        }

        public DateTime[] GetHolidays(
            int startYear,
            int endYear)
        {
            var holidays = new List<DateTime>();

            for (int yearCounter = startYear; yearCounter <= endYear; yearCounter++)
            {
                var holidaysForYear = CreateRandomNonWeekendDaysInYear(yearCounter);

                holidays.AddRange(holidaysForYear);
            }

            return holidays.ToArray();
        }

        private DateTime[] CreateRandomNonWeekendDaysInYear(
            int yearCounter)
        {
            var holidayCount = _random.Next(5, 7);

            var holidays = new DateTime[holidayCount];

            for (int holidayCounter = 0; holidayCounter < holidayCount; holidayCounter++)
            {
                holidays[holidayCounter] = GetRandomNonWeekendDateInYear(yearCounter);
            }

            return holidays;
        }

        private DateTime GetRandomNonWeekendDateInYear(
            int year)
        {
            var nonWeekendDate = CreateRandomDayInYear(year);

            while (IsWeekend(nonWeekendDate))
            {
                nonWeekendDate = CreateRandomDayInYear(year);
            }

            return nonWeekendDate;
        }

        private DateTime CreateRandomDayInYear(
            int year)
        {
            var dayOffset = GetRandomDayCountInYear();

            return new DateTime(year, 1, 1)
                .AddDays(dayOffset);
        }

        private int GetRandomDayCountInYear()
        {
            return _random.Next(0, 364);
        }

        private static bool IsWeekend(
            DateTime d)
        {
            return d.DayOfWeek == DayOfWeek.Saturday
                   || d.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}