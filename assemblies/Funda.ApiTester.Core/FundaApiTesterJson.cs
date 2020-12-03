using Funda.ApiTester.Client;
using Funda.ApiTester.Core.Interfaces;
using Funda.ApiTester.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.ApiTester.Core
{
    public class FundaApiTesterJson : IFundaApiTester
    {
        private IFundaApiClient _fundaApiClient;
        private IRequestBuilder _requestBuilder;

        public FundaApiTesterJson(IFundaApiClient fundaApiClient, IRequestBuilder requestBuilder)
        {
            _fundaApiClient = fundaApiClient ?? throw new ArgumentNullException(nameof(fundaApiClient));
            _requestBuilder = requestBuilder ?? throw new ArgumentNullException(nameof(requestBuilder));
        }

        public async Task<IDictionary<RealEstateAgent, int>> GetTopXAgentsByNumberOFListingsForSaleAsync(int limit, string region)
        {
            return await GetTopXAgentsByNumberOFListingsWithGardenAsyncImpl(limit, region);
        }

        public async Task<IDictionary<RealEstateAgent, int>> GetTopXAgentsByNumberOFListingsForSaleWithGardenAsync(int limit, string region)
        {
            return await GetTopXAgentsByNumberOFListingsWithGardenAsyncImpl(limit, region, true);
        }

        private async Task<IDictionary<RealEstateAgent, int>> GetTopXAgentsByNumberOFListingsWithGardenAsyncImpl(int limit, string region, bool withGarden = false)
        {
            if (limit == 0 || string.IsNullOrWhiteSpace(region)) return null;
            var currentPage = 1;

            var requestUri = _requestBuilder.BuildRequestUri(region, ListingType.Sale, ResponseContentType.Json, currentPage, withGarden);
            var result = await _fundaApiClient.GetAsync(requestUri);

            var cumulativeResult = ProcessResult(result, out var nextPageAvailable);

            if (!nextPageAvailable) return cumulativeResult;

            while (nextPageAvailable)
            {
                currentPage++;
                requestUri = _requestBuilder.BuildRequestUri(region, ListingType.Sale, ResponseContentType.Json, currentPage, withGarden);
                var newResult = await _fundaApiClient.GetAsync(requestUri);

                if (newResult.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Thread.Sleep(30000);
                    currentPage--;
                    continue;
                }

                var pageResult = ProcessResult(newResult, out nextPageAvailable);

                if (pageResult == null) return pageResult;

                cumulativeResult = cumulativeResult.Concat(pageResult).GroupBy(o => o.Key).ToDictionary(o => o.Key, o => o.Sum(v => v.Value));
            }

            var top = cumulativeResult.OrderByDescending(pair => pair.Value).Take(limit);
            return top.ToDictionary(item => item.Key, item => item.Value);
        }

        private Dictionary<RealEstateAgent, int> ProcessResult(FundaApiResult result, out bool nextPageAvailable)
        {
            nextPageAvailable = false;

            var jsonContent = JObject.Parse(result.Content);

            if (jsonContent == null) return null;

            var listings = jsonContent["Objects"];
            var processedResult = ProcessListings(listings);

            var paging = jsonContent["Paging"];
            nextPageAvailable = HasNextPage(paging);

            return processedResult;
        }

        private Dictionary<RealEstateAgent, int> ProcessListings(JToken listingsJson)
        {
            var processedResult = new Dictionary<RealEstateAgent, int>();
            foreach (var listing in listingsJson)
            {
                if (!int.TryParse(listing["MakelaarId"].ToString(), out var agentId)) continue;
                var agentName = listing["MakelaarNaam"].ToString();

                var realEstateAgent = new RealEstateAgent
                {
                    Id = agentId,
                    Name = agentName
                };

                if (processedResult.ContainsKey(realEstateAgent))
                {
                    processedResult[realEstateAgent]++;
                    continue;
                }

                processedResult.Add(realEstateAgent, 1);
            }

            return processedResult;
        }

        private bool HasNextPage(JToken pagingJson)
        {
            if (!int.TryParse(pagingJson["HuidigePagina"].ToString(), out var currentPage)) return false;
            if (!int.TryParse(pagingJson["AantalPaginas"].ToString(), out var totalPages)) return false;
            if (totalPages < 1) return false;

            return currentPage < totalPages;
        }
    }
}
