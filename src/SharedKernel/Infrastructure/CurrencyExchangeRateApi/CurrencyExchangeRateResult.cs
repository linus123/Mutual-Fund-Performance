using System;

namespace MutualFundPerformance.SharedKernel.Infrastructure.CurrencyExchangeRateApi
{
    public class CurrencyExchangeRateResult
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BaseCurrencyCode { get; set; }

        public DateAndRate[] Rates { get; set; }

        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }

        public class DateAndRate
        {
            public DateTime Date { get; set; }
            public decimal Rate { get; set; }
        }
    }
}