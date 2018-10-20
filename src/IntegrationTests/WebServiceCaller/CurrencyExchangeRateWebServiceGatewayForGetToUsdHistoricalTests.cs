using System;
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

            var results = gateway.GetToUsdHistorical(new CurrencyExchangeRateRequest[0]);

            results.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldThrowExceptionWhenCurrencyNotProvide()
        {
            var gateway = new CurrencyExchangeRateWebServiceGateway();

            var requests = new []
            {
                new CurrencyExchangeRateRequest(),
            };

            Assert.Throws<Exception>(() =>
            {
                gateway.GetToUsdHistorical(requests);
            });

        }
    }
}