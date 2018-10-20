using System;
using MutualFundPerformance.SharedKernel.Infrastructure.CurrencyExchangeRateApi;

namespace MutualFundPerformance.WebServiceCaller
{
    public class CurrencyExchangeRateWebServiceGateway
    {
        public CurrencyExchangeRateResult[] GetToUsdHistorical(
            CurrencyExchangeRateRequest[] requests)
        {
            foreach (var request in requests)
            {
                if (string.IsNullOrEmpty(request.BaseCurrencyCode))
                {
                    throw new Exception("Currency code has not been given for one or more requests.");
                }
            }

            return new CurrencyExchangeRateResult[0];
        }
    }
}