﻿using System;
using System.Collections.Generic;
using System.Linq;
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

            var results = new List<CurrencyExchangeRateResult>();
            
            foreach (var request in requests)
            {
                if (string.IsNullOrEmpty(request.BaseCurrencyCode))
                {
                    throw new Exception("Currency code has not been given for one or more requests.");
                }

                var nextResult = CreateResult(request);

                results.Add(nextResult);
            }

            return results.ToArray();
        }

        private CurrencyExchangeRateResult CreateResult(
            CurrencyExchangeRateRequest request)
        {
            var validCodes = GetValidCodes();

            if (validCodes.Any(c => c == request.BaseCurrencyCode))
            {
                var dateAndRates = CreateDatesAndRates(request);

                return CreateValidResult(
                    dateAndRates,
                    request);
            }

            return CreateResultWithError(
                "Could not find rates for given currency code.",
                request);
        }

        private static CurrencyExchangeRateResult.DateAndRate[] CreateDatesAndRates(
            CurrencyExchangeRateRequest request)
        {
            var random = new Random(request.BaseCurrencyCode.GetHashCode());

            var dateCounter = request.StartDate;

            var dateAndRates = new List<CurrencyExchangeRateResult.DateAndRate>();

            var nextRate = CreateInitialRate(random);

            while (dateCounter <= request.EndDate)
            {
                if (!IsWeekend(dateCounter))
                {
                    dateAndRates.Add(new CurrencyExchangeRateResult.DateAndRate()
                    {
                        Date = dateCounter,
                        Rate = nextRate
                    });

                    nextRate = nextRate + CreateVariance(random);
                }

                dateCounter = dateCounter.AddDays(1);
            }

            return dateAndRates.ToArray();
        }

        private static decimal CreateVariance(Random random)
        {
            return (Convert.ToDecimal(random.Next(-1000, 1000)) / 100000m);
        }

        private static decimal CreateInitialRate(Random random)
        {
            return 0.5m + random.Next(0, 1500000) / 1000000m;
        }

        private static CurrencyExchangeRateResult CreateValidResult(
            CurrencyExchangeRateResult.DateAndRate[] andRates,
            CurrencyExchangeRateRequest request)
        {
            return new CurrencyExchangeRateResult()
            {
                BaseCurrencyCode = request.BaseCurrencyCode,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Rates = andRates,
                HasError = false,
            };
        }

        private static CurrencyExchangeRateResult CreateResultWithError(
            string errorMessage,
            CurrencyExchangeRateRequest request)
        {
            return new CurrencyExchangeRateResult()
            {
                BaseCurrencyCode = request.BaseCurrencyCode,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                HasError = true,
                ErrorMessage = errorMessage
            };
        }

        private static bool IsWeekend(DateTime dateCounter)
        {
            return dateCounter.DayOfWeek == DayOfWeek.Saturday || dateCounter.DayOfWeek == DayOfWeek.Sunday;
        }

        private string[] GetValidCodes()
        {
            return new[]
            {
                "GBP",
                "EUR",
                "CAD",
                "CNY",
                "AUD",
                "DEM",
                "FRF",
                "ITL",
                "NOK",
                "CHF",
                "NZD",
                "CHF",
                "KRW"
            };
        }
    }
}