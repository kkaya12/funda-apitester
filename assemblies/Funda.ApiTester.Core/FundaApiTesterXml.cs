using Funda.ApiTester.Core.Interfaces;
using Funda.ApiTester.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funda.ApiTester.Core
{
    public class FundaApiTesterXml : IFundaApiTester
    {
        public Task<IDictionary<RealEstateAgent, int>> GetTopXAgentsByNumberOFListingsForSaleAsync(int limit, string region)
        {
            throw new System.NotImplementedException();
        }

        public Task<IDictionary<RealEstateAgent, int>> GetTopXAgentsByNumberOFListingsForSaleWithGardenAsync(int limit, string region)
        {
            throw new System.NotImplementedException();
        }
    }
}
