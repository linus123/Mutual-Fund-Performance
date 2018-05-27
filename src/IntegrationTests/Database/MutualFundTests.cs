using System;
using FluentAssertions;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.Database
{
    public class MutualFundTests
    {
        private readonly IntegrationTestsSettings _integrationTestsSettings;

        public MutualFundTests()
        {
            _integrationTestsSettings = new IntegrationTestsSettings();
        }

        [Fact]
        public void MutualFundDataTableGatewayShouldPersistData()
        {
            var mutualFundDataTableGateway = new MutualFundDataTableGateway(_integrationTestsSettings);

            mutualFundDataTableGateway.DeleteAll();

            var mutualFundId = Guid.NewGuid();
            
            var dto1 = new MutualFundDto()
            {
                MutualFundId = mutualFundId,
                Name = "Mutual Fund 1"
            };

            mutualFundDataTableGateway.Insert(new []{ dto1 });

            var mutualFundDtos = mutualFundDataTableGateway.GetAll();

            mutualFundDtos.Should().HaveCount(1);

            mutualFundDtos[0].MutualFundId.Should().Be(mutualFundId);
            mutualFundDtos[0].Name.Should().Be("Mutual Fund 1");

            mutualFundDataTableGateway.DeleteAll();
        }

        [Fact]
        public void BenchmarkDataTableGatewayShouldPersistData()
        {
            var mutualFundDataTableGateway = new MutualFundDataTableGateway(_integrationTestsSettings);
            var benchmarkDataTableGateway = new BenchmarkDataTableGateway(_integrationTestsSettings);

            benchmarkDataTableGateway.DeleteAll();
            mutualFundDataTableGateway.DeleteAll();

            var mutualFundId = Guid.NewGuid();

            var mutualFund1 = new MutualFundDto()
            {
                MutualFundId = mutualFundId,
                Name = "Mutual Fund 1"
            };

            mutualFundDataTableGateway.Insert(new[] { mutualFund1 });

            var benchmark1Id = Guid.NewGuid();

            var benchmark1 = new BenchmarkDto()
            {
                BenchmarkId = benchmark1Id,
                MutualFundId = mutualFundId,
                Name = "Benchamrk 1"
            };

            benchmarkDataTableGateway.Insert(new []{ benchmark1 });

            var benchmarkDtos = benchmarkDataTableGateway.GetAll();

            benchmarkDtos.Should().HaveCount(1);

            benchmarkDtos[0].BenchmarkId.Should().Be(benchmark1Id);
            benchmarkDtos[0].Name.Should().Be("Benchamrk 1");
            benchmarkDtos[0].MutualFundId.Should().Be(mutualFundId);

            benchmarkDataTableGateway.DeleteAll();
            mutualFundDataTableGateway.DeleteAll();
        }

    }
}