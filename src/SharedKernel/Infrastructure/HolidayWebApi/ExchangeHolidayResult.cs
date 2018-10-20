using System;

namespace MutualFundPerformance.SharedKernel.Infrastructure.HolidayWebApi
{
    public class ExchangeHolidayResult
    {
        public string ExchangeIsoCode { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }

        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public DateTime[] Holidays { get; set; }
    }
}