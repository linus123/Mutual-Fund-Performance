using System;

namespace MutualFundPerformance.WebApi.Models
{
    public struct VehicleSummaryRequest
    {
        public DateStruct Date { get; set; }
        public Guid[] MutualFundIds { get; set; }
    }

    public struct DateStruct
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}