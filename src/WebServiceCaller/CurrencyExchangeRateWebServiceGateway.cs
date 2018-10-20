using System;
using MutualFundPerformance.SharedKernel.Infrastructure.CurrencyExchangeRateApi;

namespace MutualFundPerformance.WebServiceCaller
{
    public class CurrencyExchangeRateWebServiceGateway
    {
        public CurrencyExchangeRateResult[] GetToUsdHistorical(
            CurrencyExchangeRateRequest[] requests)
        {
            if (requests.Length <= 0)
                return new CurrencyExchangeRateResult[0];

            foreach (var request in requests)
            {
                if (string.IsNullOrEmpty(request.BaseCurrencyCode))
                {
                    throw new Exception("Currency code has not been given for one or more requests.");
                }
            }

            return new CurrencyExchangeRateResult[]
            {
                new CurrencyExchangeRateResult()
                {
                    BaseCurrencyCode = requests[0].BaseCurrencyCode,
                    StartDate = requests[0].StartDate,
                    EndDate = requests[0].EndDate,
                    HasError = true,
                    ErrorMessage = "Could not find rates for given currency code."
                }
            };
        }
    }
}