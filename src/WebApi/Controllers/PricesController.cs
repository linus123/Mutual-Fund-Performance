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

        /*

        {
  {
    "ColumnHeadings" : [
      "2018-01-10",
      "2018-01-09",
      "2018-01-08",
      "2018-01-07",
      "2018-01-06",
      ]
  },
  [
    {
      "Id" : "5153513-21351-5351-6511351",
      "MutualFundName" : "My fund 1",
       "EndDateMinus0": 1.20,
       "EndDateMinus1": 1.21,
       "EndDateMinus2": 1.22,
       "EndDateMinus3": 1.23,
       "EndDateMinus4": 1.24,
       
    },
    {
      "Id" : "4565-456-5556351-5",
      "MutualFundName" : "My fund 2",
       "EndDateMinus0": 1.20,
       "EndDateMinus2": 1.22,
       "EndDateMinus3": 1.23,
       "EndDateMinus4": 1.24,
       
    },
     {
      "Id" : "4575-456-5556351-5",
      "MutualFundName" : "My fund 3",
       "EndDateMinus3": 1.23,
       "EndDateMinus4": 1.24,
       
    },
    
  ]
}

        */

        /*
         *
         * {
  "enddate": {
    year: 2018,
    month: 10,
    day: 1
  },
  [
    "5153513-21351-5351-6511351",
    "5153513-21351-5351-6511523",
    "5153513-21351-5351-6511353"
  ]
}
         */
    }
}