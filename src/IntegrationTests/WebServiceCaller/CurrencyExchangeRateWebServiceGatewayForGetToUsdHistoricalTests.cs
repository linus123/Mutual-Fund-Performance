﻿using System;
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

        [Fact]
        public void ShouldReturnErrorWhenRatesNotFoundForCode()
        {
            var gateway = new CurrencyExchangeRateWebServiceGateway();

            var requests = new[]
            {
                new CurrencyExchangeRateRequest()
                {
                    BaseCurrencyCode = "XXX",
                    StartDate = new DateTime(2010, 1, 1),
                    EndDate = new DateTime(2018, 1, 1)
                },
            };

            var results = gateway.GetToUsdHistorical(requests);

            results.Should().HaveCount(1);

            results[0].BaseCurrencyCode.Should().Be("XXX");
            results[0].StartDate.Should().Be(new DateTime(2010, 1, 1));
            results[0].EndDate.Should().Be(new DateTime(2018, 1, 1));

            results[0].HasError.Should().BeTrue();
            results[0].ErrorMessage.Should().Be("Could not find rates for given currency code.");
        }

        [Fact]
        public void ShouldReturnRatesWhenCurrencyCodeIsValid()
        {
            var gateway = new CurrencyExchangeRateWebServiceGateway();

            var requests = new[]
            {
                new CurrencyExchangeRateRequest()
                {
                    BaseCurrencyCode = "CAD",
                    StartDate = new DateTime(2018, 10, 8),
                    EndDate = new DateTime(2018, 10, 19)
                },
            };

            var results = gateway.GetToUsdHistorical(requests);

            results.Should().HaveCount(1);

            results[0].BaseCurrencyCode.Should().Be("CAD");
            results[0].StartDate.Should().Be(new DateTime(2018, 10, 8));
            results[0].EndDate.Should().Be(new DateTime(2018, 10, 19));

            results[0].HasError.Should().BeFalse();

            results[0].Rates.Should().HaveCount(10);

            foreach (var dateAndRate in results[0].Rates)
            {
                dateAndRate.Date.Should().BeOnOrAfter(new DateTime(2018, 10, 8));
                dateAndRate.Date.Should().BeOnOrBefore(new DateTime(2018, 10, 19));

                dateAndRate.Rate.Should().BeGreaterOrEqualTo(0.5m);
                dateAndRate.Rate.Should().BeLessOrEqualTo(2.0m);
            }
        }

    }
}