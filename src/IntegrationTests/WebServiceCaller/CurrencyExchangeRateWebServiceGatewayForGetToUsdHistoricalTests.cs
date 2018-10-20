using MutualFundPerformance.SharedKernel.Infrastructure.CurrencyExchangeRateApi;
using WebServiceCaller;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.WebServiceCaller
{
    public class CurrencyExchangeRateWebServiceGatewayForGetToUsdHistoricalTests
    {
        [Fact]
        public void ShouldWork()
        {
            var gateway = new CurrencyExchangeRateWebServiceGateway();

            gateway.GetToUsdHistorical(new CurrencyExchangeRateResult[0]);
        }
    }
}