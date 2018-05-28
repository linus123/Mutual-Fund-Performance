using FluentAssertions;
using MutualFundPerformance.WebApi.Controllers;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.WepApi.Controllers
{
    public class PriceControllerTests
    {
        [Fact]
        public void ShouldReturnEmptyArrayWhenNoSymbolsAreGiven()
        {
            var controller = new PriceController();

            var result = controller.GetAll(null);

            result.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldReturnEmptyArrayWhenGivenSymbolIsNotFound()
        {
            var controller = new PriceController();

            var result = controller.GetAll("SYMB");

            result.Should().HaveCount(0);
        }

    }
}