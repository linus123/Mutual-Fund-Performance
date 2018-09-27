using System;

namespace MutualFundPerformance.WebApi.Models
{
    public struct FundPrice
    {
        public Guid FundId;
        public DateTime Date;
        public decimal? Price;
    }
}
