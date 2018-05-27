using System;

namespace MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData
{
    public class InvestmentVehicleDto
    {
        public Guid InvestmentVehicleId { get; set; }
        public string Name { get; set; }
        public Guid ExternalId { get; set; }
    }
}