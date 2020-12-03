using Funda.ApiTester.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace Funda.ApiTester.Core
{
    public class StaticRequestParametersProvider : IStaticRequestParametersProvider
    {
        private IConfiguration _config;

        public StaticRequestParametersProvider(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public string GetKey()
        {
            return _config.GetSection("Api")?["Key"];
        }

        public string GetAddressBase()
        {
            return _config.GetSection("Api")?["AddressBase"];
        }
    }
}
