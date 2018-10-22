using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;
using MutualFundPerformance.WebApi.Controllers;
using Xunit;


namespace MutualFundPerformance.IntegrationTests
{
    public class ServiceTests
    {

        [Fact]  
        public void ShouldGetNoFunds()
        {
          
            var settings = new IntegrationTestsSettings();
            var mutualFundGateway = new MutualFundDataTableGateway(new IntegrationTestsSettings());

            mutualFundGateway.DeleteAll();

            MutualFundPrice mutualFundPrice = new MutualFundPrice(settings);
            mutualFundPrice.GetAllFunds().Length.Should().Be(0);
        }

       
        [Fact]
        public void ShouldGetInsertedDataBack()
        {
            var settings = new IntegrationTestsSettings();
            var mutualFundGateway = new MutualFundDataTableGateway(new IntegrationTestsSettings());
            mutualFundGateway.DeleteAll();

            mutualFundGateway.Insert(new MutualFundDto[]
            {
                new MutualFundDto
                {
                    MutualFundId = Guid.NewGuid(),
                    Name = "Mutual Fund 1",
                    Symbol = "MUTF"
                }
            });

            var mutualFundPrice = new MutualFundPrice(settings);
            mutualFundPrice.GetAllFunds().Length.Should().Be(1);
            mutualFundPrice.GetAllFunds().Single().Should().Be("Mutual Fund 1");
        }

        [Fact]
        public void ShouldReturnFundsInOrder()
        {
            var settings = new IntegrationTestsSettings();
            var mutualFundGateway = new MutualFundDataTableGateway(new IntegrationTestsSettings());
            mutualFundGateway.DeleteAll();

            mutualFundGateway.Insert(new MutualFundDto[]
            {
                new MutualFundDto
                {
                    MutualFundId = Guid.NewGuid(),
                    Name = "Mutual Fund 1",
                    Symbol = "MUTF"
                },
                new MutualFundDto
                {
                    MutualFundId = Guid.NewGuid(),
                    Name = "A Mutual Fund 1",
                    Symbol = "AMUTF"
                }
                ,
                new MutualFundDto
                {
                    MutualFundId = Guid.NewGuid(),
                    Name = "B Mutual Fund 1",
                    Symbol = "BMUTF"
                }

            });

            var mutualFundPrice = new MutualFundPrice(settings);
            mutualFundPrice.GetAllFunds().Length.Should().Be(3);
            mutualFundPrice.GetAllFunds()[0].Should().Be("A Mutual Fund 1");
            mutualFundPrice.GetAllFunds()[1].Should().Be("B Mutual Fund 1");
            mutualFundPrice.GetAllFunds()[2].Should().Be("Mutual Fund 1");
        }
    }
}
