using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        private MutualFundDataTableGateway mutualFundGateway;
        private IntegrationTestsSettings settings;

        public ServiceTests()
        {
            settings = new IntegrationTestsSettings();
            mutualFundGateway = new MutualFundDataTableGateway(new IntegrationTestsSettings());
            mutualFundGateway.DeleteAll();
        }

        [Fact]  
        public void ShouldGetNoFunds()
        {
            MutualFundPrice mutualFundPrice = new MutualFundPrice(settings);
            mutualFundPrice.GetAllFunds().Length.Should().Be(0);
        }
       
        [Fact]
        public void ShouldGetInsertedDataBack()
        {
            mutualFundGateway.Insert(new MutualFundDto[]
            {
                new MutualFundDtoBuilder("Mutual Fund 1").Create()
            });

            var mutualFundPrice = new MutualFundPrice(settings);
            mutualFundPrice.GetAllFunds().Length.Should().Be(1);
            mutualFundPrice.GetAllFunds().Single().Name.Should().Be("Mutual Fund 1");
        }

        [Fact]
        public void ShouldReturnFundsInOrder()
        {
            mutualFundGateway.Insert(new MutualFundDto[]
            {

                new MutualFundDtoBuilder("Mutual Fund 1").Create(),
                new MutualFundDtoBuilder("A Mutual Fund 1").Create(),
                new MutualFundDtoBuilder("B Mutual Fund 1").Create()
            });

            var mutualFundPrice = new MutualFundPrice(settings);
            mutualFundPrice.GetAllFunds().Length.Should().Be(3);
            mutualFundPrice.GetAllFunds()[0].Name.Should().Be("A Mutual Fund 1");
            mutualFundPrice.GetAllFunds()[1].Name.Should().Be("B Mutual Fund 1");
            mutualFundPrice.GetAllFunds()[2].Name.Should().Be("Mutual Fund 1");
        }

        [Fact]
        public void ShouldReturnMutualFundID()
        {
            Guid newGuid = Guid.NewGuid();
            mutualFundGateway.Insert(new MutualFundDto[]
            {
                new MutualFundDtoBuilder("Mutual Fund 1",newGuid).Create()
            });

            var mutualFundPrice = new MutualFundPrice(settings);
            mutualFundPrice.GetAllFunds().Length.Should().Be(1);
            mutualFundPrice.GetAllFunds()[0].Id.Should().Be(newGuid);

        }
    }
}
