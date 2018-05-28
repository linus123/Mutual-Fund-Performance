using FluentAssertions;
using MutualFundPerformance.WebApi.Controllers;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.WepApi.Controllers
{
    public class PriceControllerTests
    {
        [Fact]
        public void ShouldReturnNoResultWhenNothingIsProvied()
        {
            var controller = new PriceController();

            var result = controller.GetAll();

            result.Should().HaveCount(0);
        }
    }
}