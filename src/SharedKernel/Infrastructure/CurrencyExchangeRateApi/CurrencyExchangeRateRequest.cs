using System;

namespace MutualFundPerformance.SharedKernel.Infrastructure.CurrencyExchangeRateApi
{
    public class CurrencyExchangeRateRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BaseCurrencyCode { get; set; }
    }
}