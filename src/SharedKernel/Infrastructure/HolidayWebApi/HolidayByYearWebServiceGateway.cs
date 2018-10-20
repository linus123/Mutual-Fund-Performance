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
                var holidayCount = _random.Next(5, 7);

                for (int holidayCounter = 0; holidayCounter < holidayCount; holidayCounter++)
                {
                    var dayOffset = _random.Next(0, 364);

                    var possibleDate = new DateTime(yearCounter, 1, 1)
                        .AddDays(dayOffset);

                    while (possibleDate.DayOfWeek == DayOfWeek.Saturday || possibleDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dayOffset = _random.Next(0, 364);

                        possibleDate = new DateTime(yearCounter, 1, 1)
                            .AddDays(dayOffset);
                    }

                    holidays.Add(possibleDate);
                }
            }

            return holidays.ToArray();
        }
    }
}