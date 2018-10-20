using FluentAssertions;
using MutualFundPerformance.SharedKernel.Infrastructure.CurrencyExchangeRateApi;
using MutualFundPerformance.WebServiceCaller;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.WebServiceCaller
{
    public class CurrencyExchangeRateWebServiceGatewayForGetToUsdHistoricalTests
    {
        [Fact]
        public void ShouldReturnEmptyArrayGivenEmptyArray()
        {
            var gateway = new CurrencyExchangeRateWebServiceGateway();

            var results = gateway.GetToUsdHistorical(new CurrencyExchangeRateResult[0]);

            results.Should().HaveCount(0);
        }
    }
}