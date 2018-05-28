using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MutualFundPerformance.Database.HistoricalPriceData;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.TestDataPopulator
{
    public class Program
    {
        static void Main(string[] args)
        {
            var currentDate = DateTime.Now;

            var testDataPopulatorSettings = new TestDataPopulatorSettings();

            var mutualFundDataTableGateway = new MutualFundDataTableGateway(testDataPopulatorSettings);
            var benchmarkDataTableGateway = new BenchmarkDataTableGateway(testDataPopulatorSettings);
            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(testDataPopulatorSettings);
            var priceDataTableGateway = new PriceDataTableGateway(testDataPopulatorSettings);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
            benchmarkDataTableGateway.DeleteAll();
            mutualFundDataTableGateway.DeleteAll();

            var mutualFundDtos = new List<MutualFundDto>();
            var benchmarkDtos = new List<BenchmarkDto>();
            var investmentVehicleDtos = new List<InvestmentVehicleDto>();
            var priceDtos = new List<PriceDto>();

            var random = new Random(100);

            var fundCount = 10;

            for (int fundCounter = 0; fundCounter < fundCount; fundCounter++)
            {
                var mutualFundId = Guid.NewGuid();

                mutualFundDtos.Add(new MutualFundDto()
                {
                    MutualFundId = mutualFundId,
                    Name = $"Mutual Fund {fundCounter}"
                });

                var mutualFundInvestmentVehicleId = Guid.NewGuid();

                investmentVehicleDtos.Add(new InvestmentVehicleDto()
                {
                    InvestmentVehicleId = mutualFundInvestmentVehicleId,
                    ExternalId = mutualFundId,
                    Name = $"Prices for Mutual Fund {fundCounter}"
                });

                priceDtos = AddPrices(currentDate, random, priceDtos, mutualFundInvestmentVehicleId);

                var benchmarkCount = 1;

                var multiBenchmarkRandom = random.Next(1, 10);

                if (multiBenchmarkRandom == 10)
                {
                    benchmarkCount = 3;
                }
                else if (multiBenchmarkRandom >= 8)
                {
                    benchmarkCount = 3;
                }

                for (int benchmarkCounter = 0; benchmarkCounter < benchmarkCount; benchmarkCounter++)
                {
                    benchmarkDtos.Add(new BenchmarkDto()
                    {
                        BenchmarkId = Guid.NewGuid(),
                        MutualFundId = mutualFundId,
                        Name = $"Benchmark {fundCounter} {benchmarkCounter}",
                        SortOrder = benchmarkCounter
                    });

                    var benchmarkInvestmentVehicleId = Guid.NewGuid();

                    investmentVehicleDtos.Add(new InvestmentVehicleDto()
                    {
                        InvestmentVehicleId = benchmarkInvestmentVehicleId,
                        ExternalId = mutualFundId,
                        Name = $"Prices for Benchmark {fundCounter} {benchmarkCounter}"
                    });

                    priceDtos = AddPrices(currentDate, random, priceDtos, benchmarkInvestmentVehicleId);

                }
            }

            Console.WriteLine("Inserting mutual funds.");
            mutualFundDataTableGateway.Insert(mutualFundDtos.ToArray());

            Console.WriteLine("Inserting benchmarks.");
            benchmarkDataTableGateway.Insert(benchmarkDtos.ToArray());

            Console.WriteLine("Inserting investment vehcles.");
            investmentVehicleDataTableGateway.Insert(investmentVehicleDtos.ToArray());

            Console.WriteLine("Inserting prices.");
            priceDataTableGateway.Insert(priceDtos.ToArray());
        }

        private static List<PriceDto> AddPrices(
            DateTime currentDate,
            Random random,
            List<PriceDto> priceDtos,
            Guid investmentVehicleId)
        {
            var dateCounter = currentDate.AddMonths(-121);
            var currentPrice = random.Next(1, 100000);

            while (dateCounter <= currentDate)
            {
                if (dateCounter.DayOfWeek != DayOfWeek.Saturday && dateCounter.DayOfWeek != DayOfWeek.Sunday)
                {
                    priceDtos.Add(new PriceDto()
                    {
                        InvestmentVehicleId = investmentVehicleId,
                        CloseDate = dateCounter.Date,
                        Price = currentPrice / 100m
                    });

                    var change = random.Next(-1000, 1000);

                    currentPrice += change;
                }

                dateCounter = dateCounter.AddDays(1);
            }

            return priceDtos;
        }
    }
}
