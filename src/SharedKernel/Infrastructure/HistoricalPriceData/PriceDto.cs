using System;

namespace MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData
{
    public class PriceDto
    {
        public Guid InvestmentVehicleId { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal Price { get; set; }
    }
}