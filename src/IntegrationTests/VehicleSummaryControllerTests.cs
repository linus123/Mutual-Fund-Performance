using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MutualFundPerformance.Database.HistoricalPriceData;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using MutualFundPerformance.WebApi.Controllers;
using MutualFundPerformance.WebApi.Models;
using Xunit;

namespace MutualFundPerformance.IntegrationTests
{
    public class VehicleSummaryControllerTests
    {
        [Fact]
        public void VehicleSummaryByDayShouldReturnNoValuesWhenNoIdOrDateMatches()
        {
            var controller = new VehicleSummaryController();
            var request = new VehicleSummaryRequest
            {
                Date = new DateStruct
                {
                    Year = 2018,
                    Month = 12,
                    Day = 15,
                },
                MutualFundIds = new[] { Guid.Empty }
            };

            var result = controller.ByDay(request);

            result.Should().NotBeNull();
        }

        [Fact]
        public void VehicleSummaryByDayShouldReturnNoValuesWhenDateIsInvalid()
        {
            var controller = new VehicleSummaryController();
            var request = new VehicleSummaryRequest
            {
                Date = new DateStruct
                {
                    Year = -2018,
                    Month = -12,
                    Day = -15,
                },
                MutualFundIds = new[] { Guid.Empty }
            };

            var result = controller.ByDay(request);

            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public void VehicleSummaryByDayShouldReturnAnEmptyArrayWhenNoGuidsAreSent()
        {
            var controller = new VehicleSummaryController();
            var request = new VehicleSummaryRequest
            {
                Date = new DateStruct
                {
                    Year = 2018,
                    Month = 12,
                    Day = 15,
                },
                MutualFundIds = new Guid[0]
            };

            var result = controller.ByDay(request);

            ((object[])((OkObjectResult)result).Value).Length.Should().Be(0);
        }

        /*
         *
         *{
  Date {
    Year: 2018,
    Month: 2,
    Day: 2
  },
  MutualFundIds: ["Guid1", "Guid2"]
}
         *
         */

        /*
[

  {
    Id: "Guid1",
    Name: "MF1",
    Price: 3000.2,
    OneDayReturn: 0.003,
    WeekToDate: 0.0005,
    MonthToDate: 0.0008,
    OneMonth: {
      IsValid: true,
      Value: 0.0002,
      ErrorMessage: null
    },
    ThreeMonth: {
      IsValid: false,
      Value: null,
      ErrorMessage: "Missing 1 of 3 returns"
    }
  },
    {
    Id: "Guid2",
    Name: "MF2",
    Price: 3000.2,
    OneDayReturn: 0.003,
    WeekToDate: 0.0005,
    MonthToDate: 0.0008,
    OneMonth: {
      IsValid: true,
      Value: 0.0002,
      ErrorMessage: null
    },
    ThreeMonth: {
      IsValid: false,
      Value: null,
      ErrorMessage: "Missing 1 of 3 returns"
    }
  }

]
         */

    }
}