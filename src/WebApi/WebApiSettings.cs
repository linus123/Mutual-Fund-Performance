using Microsoft.Extensions.Configuration;
using MutualFundPerformance.SharedKernel;

namespace MutualFundPerformance.WebApi
{
    public class WebApiSettings : IMutualFundPerformanceDatabaseSettings
    {
        private readonly IConfiguration _configuration;

        public WebApiSettings(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string MutualFundPerformanceDatabaseConnectionString
        {
            get { return _configuration.GetConnectionString("MutualFundPerformanceDatabase"); }
        }
    }
}