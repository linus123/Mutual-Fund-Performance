using System;

namespace MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData
{
    public class BenchmarkDto
    {
        public Guid BenchmarkId { get; set; }
        public string Name { get; set; }
        public Guid MutualFundId { get; set; }

    }
}