using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData
{
    public class MutualFundPriceService
    {
        private readonly IMutualFundDataGateway _mutualFundDataTableGateway;
        
        public MutualFundPriceService(IMutualFundDataGateway mutualFundDataTable)
        {
            _mutualFundDataTableGateway = mutualFundDataTable;
        }

        public FundListModel[] GetAllFunds()
        {
            var mutualFundDtos = _mutualFundDataTableGateway.GetAll();

            var fundNames = mutualFundDtos
                .Select(f => new FundListModel
                {
                    Id = f.MutualFundId,
                    Name = f.Name
                })
                .OrderBy(m => m.Name)
                .ToArray();

            return fundNames;
        }
    }

    public class FundListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
