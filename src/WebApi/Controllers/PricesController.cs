using System.Linq;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.WebApi.Controllers
{
    public class PricesController
    {
        
        private MutualFundPricesService _mutualFundPriceService;
        public PricesController(
            MutualFundPricesService mutualFundPriceService
            )
        {
            _mutualFundPriceService = mutualFundPriceService;
        }

        public MutualFundDto[] Funds()
        {
            return _mutualFundPriceService.Funds();

        }
    }
}