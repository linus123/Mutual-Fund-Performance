using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;

namespace MutualFundPerformance.SharedKernel.Performance
{
    public class MutualFundRepository
    {
        private IPriceDataTableGateway _priceDataTableGateway;
        private readonly IMutualFundDataGateway _mutualFundDataTableGateway;
        private readonly IInvestmentVehicleDataTableGateway _investmentVehicleDataTableGateway;

        public MutualFundRepository(IPriceDataTableGateway priceDataTableGateway,
            IMutualFundDataGateway mutualFundDataGateway,
            IInvestmentVehicleDataTableGateway investmentVehicleDataTableGateway)
        {
            _priceDataTableGateway = priceDataTableGateway;
            _mutualFundDataTableGateway = mutualFundDataGateway;
            _investmentVehicleDataTableGateway = investmentVehicleDataTableGateway;
        }

        public PerformanceService.MutualFundObject[] GetMutualFundObjects()
        {
            List<PerformanceService.MutualFundObject> returnList = new List<PerformanceService.MutualFundObject>();
            var mutualFundDtos = _mutualFundDataTableGateway.GetAll();
            var investmentVehicleDtos = _investmentVehicleDataTableGateway.GetAll();

            var priceDtos = _priceDataTableGateway.GetAll();
            
            foreach (var mutualFundDto in mutualFundDtos)
            {
                var investVehicleDto = investmentVehicleDtos.Where(i => i.ExternalId == mutualFundDto.MutualFundId).First();

                var priceDto = priceDtos.Where(p => p.InvestmentVehicleId == investVehicleDto.InvestmentVehicleId);

                var mutualFundObject = new PerformanceService.MutualFundObject()
                {
                    Name = mutualFundDto.Name,
                    Id = mutualFundDto.MutualFundId,
                    Prices = priceDto.ToList()
                };

                returnList.Add(mutualFundObject);
            }

            return returnList.ToArray();
        }

    }
}
