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

            var mutualFundDataTableGateway = new MutualFundDataTableGateway(
                testDataPopulatorSettings);

            mutualFundDataTableGateway.DeleteAll();

            var mutualFundDtos = new List<MutualFundDto>();

            for (int fundCount = 0; fundCount < 20; fundCount++)
            {
                mutualFundDtos.Add(new MutualFundDto()
                {
                    MutualFundId = Guid.NewGuid(),
                    Name = $"Mutual Fund {fundCount}"
                });
            }

            Console.WriteLine("Inserting mutual funds.");

            mutualFundDataTableGateway.Insert(
                mutualFundDtos.ToArray());
        }
    }
}
