﻿using System.IO;
using Microsoft.Extensions.Configuration;
using MutualFundPerformance.SharedKernel;

namespace MutualFundPerformance.IntegrationTests
{
    public class IntegrationTestsSettings : IMutualFundPerformanceDatabaseSettings
    {
        public string MutualFundPerformanceDatabaseConnectionString
        {
            get
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                var configurationRoot = configurationBuilder.Build();

                return configurationRoot.GetConnectionString("MutualFundPerformanceDatabase");
            }
        }
    }
}
