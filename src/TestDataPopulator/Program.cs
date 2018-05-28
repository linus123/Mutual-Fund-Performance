using System;
using System.Collections.Generic;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.TestDataPopulator
{
    class Program
    {
        static void Main(string[] args)
        {
            var testDataPopulatorSettings = new TestDataPopulatorSettings();

            var mutualFundDataTableGateway = new MutualFundDataTableGateway(testDataPopulatorSettings);
            var benchmarkDataTableGateway = new BenchmarkDataTableGateway(testDataPopulatorSettings);

            benchmarkDataTableGateway.DeleteAll();
            mutualFundDataTableGateway.DeleteAll();

            var mutualFundDtos = new List<MutualFundDto>();
            var benchmarkDtos = new List<BenchmarkDto>();

            var random = new Random(100);

            var fundCount = 100;

            for (int fundCounter = 0; fundCounter < fundCount; fundCounter++)
            {
                var mutualFundId = Guid.NewGuid();

                mutualFundDtos.Add(new MutualFundDto()
                {
                    MutualFundId = mutualFundId,
                    Name = $"Mutual Fund {fundCounter}"
                });

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
                }
            }

            Console.WriteLine("Inserting mutual funds.");
            mutualFundDataTableGateway.Insert(mutualFundDtos.ToArray());

            Console.WriteLine("Inserting benchmarks.");
            benchmarkDataTableGateway.Insert(benchmarkDtos.ToArray());
        }
    }
}
