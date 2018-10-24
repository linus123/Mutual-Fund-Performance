using System.Linq;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.SharedKernel
{
    public class MutualFundPricesService
    {
        private IMutualFundDataTableGateway _mutualFundDataTableGateway;
        public MutualFundPricesService(
           IMutualFundDataTableGateway mutualFundDataTableGateway
        )
        {
            _mutualFundDataTableGateway = mutualFundDataTableGateway;
        }

        public MutualFundDto[] Funds()
        {
            var mutualFundDtos = _mutualFundDataTableGateway
                .GetAll()
                .OrderBy(f => f.Name)
                .ToArray();

            return mutualFundDtos;
        }

    }
}