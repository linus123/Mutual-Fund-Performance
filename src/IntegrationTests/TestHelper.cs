using System;
using MutualFundPerformance.Database.HistoricalPriceData;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;
using MutualFundPerformance.WebApi.Controllers;

namespace MutualFundPerformance.IntegrationTests
{
    public class TestHelper
    {
        private MutualFundDataTableGateway mutualFundGateway;
        private IntegrationTestsSettings settings;
        private InvestmentVehicleDataTableGateway investmentVehicleDataTableGateway;

        public TestHelper()
        {
            settings = new IntegrationTestsSettings();
            mutualFundGateway = new MutualFundDataTableGateway(new IntegrationTestsSettings());
            investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(settings);
        }

        public void InsertMutualFundDto(
            MutualFundDto dto)
        {
            mutualFundGateway.Insert(new []{dto});
        }

        public void Reset(Action act)
        {
            DeleteAll();

            act();

            DeleteAll();
        }

        public MutualFundPriceController CreateController()
        {
            return new MutualFundPriceController(settings, investmentVehicleDataTableGateway);
        }


        public void InsertInvestmentVehicleDto(InvestmentVehicleDto dto)
        {
            investmentVehicleDataTableGateway.Insert(new InvestmentVehicleDto[]{ dto});
        }

        private void DeleteAll()
        {
            mutualFundGateway.DeleteAll();
        }
    }
}