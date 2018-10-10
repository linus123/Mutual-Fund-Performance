using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.WebApi.Controllers
{
    public class PricesController
    {
        private IMutualFundPerformanceDatabaseSettings _mutualFundPerformanceDatabaseSettings;

        public PricesController(
            IMutualFundPerformanceDatabaseSettings mutualFundPerformanceDatabaseSettings)
        {
            _mutualFundPerformanceDatabaseSettings = mutualFundPerformanceDatabaseSettings;
        }

        public MutualFundDto[] Funds()
        {
            var mutualFundDataTableGateway = new MutualFundDataTableGateway(
                _mutualFundPerformanceDatabaseSettings);

            var mutualFundDtos = mutualFundDataTableGateway.GetAll();

            return mutualFundDtos;
        }
    }
}