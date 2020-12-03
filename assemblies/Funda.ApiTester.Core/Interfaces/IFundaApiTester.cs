using Funda.ApiTester.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funda.ApiTester.Core.Interfaces
{
    public interface IFundaApiTester
    {
        Task<IDictionary<RealEstateAgent, int>> GetTopXAgentsByNumberOFListingsForSaleAsync(int limit, string region);
        Task<IDictionary<RealEstateAgent, int>> GetTopXAgentsByNumberOFListingsForSaleWithGardenAsync(int limit, string region);
    }
}
