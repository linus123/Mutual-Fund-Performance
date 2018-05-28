using System;

namespace MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData
{
    public class MutualFundDto
    {
        public Guid MutualFundId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}