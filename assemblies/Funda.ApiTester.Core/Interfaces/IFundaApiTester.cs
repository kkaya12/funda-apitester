using Funda.ApiTester.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funda.ApiTester.Core.Interfaces
{
    public interface IFundaApiTester
    {
        /// <summary>
        /// Returns the top real estate agents with the most number of listings in the specified region.
        /// </summary>
        /// <param name="limit">The number of agents to return.</param>
        /// <param name="region">The region to query.</param>
        /// <returns></returns>
        Task<IDictionary<RealEstateAgent, int>> GetTopXAgentsByNumberOFListingsForSaleAsync(int limit, string region);

        /// <summary>
        /// Returns the top real estate agents with the most number of listings with garden in the specified region.
        /// </summary>
        /// <param name="limit">The number of agents to return.</param>
        /// <param name="region">The region to query.</param>
        /// <returns></returns>
        Task<IDictionary<RealEstateAgent, int>> GetTopXAgentsByNumberOFListingsForSaleWithGardenAsync(int limit, string region);
    }
}
