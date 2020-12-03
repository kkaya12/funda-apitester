using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Funda.ApiTester.Client
{
    public class FundaApiHttpClient : IFundaApiClient
    {
        private HttpClient _httpClient;
        public FundaApiHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory?.CreateClient(nameof(FundaApiHttpClient)) ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        ///<inheritdoc cref="IFundaApiClient"/>
        public async Task<FundaApiResult> GetAsync(Uri uri)
        {
            using var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = HttpMethod.Get,
            };

            return await SendHttpRequestAsync(httpRequestMessage);
        }

        private async Task<FundaApiResult> SendHttpRequestAsync(HttpRequestMessage message)
        {
            try
            {
                var response = await _httpClient.SendAsync(message);

                //TODO: Using response.EnsureSuccessStatusCode() would make this tidier, but by intuiton branching using exceptions is expensive?
                //If we're to handle cases of exceptions separetely, I think this is better (e.g. different messages for 4XX status codes?)
                if (!response.IsSuccessStatusCode)
                {
                    return FundaApiResult.CreateErrorResult($"Request failed with status code: {response.StatusCode}", response.StatusCode);
                }

                return new FundaApiResult
                {
                    IsSuccesss = true,
                    Content = await response.Content.ReadAsStringAsync()
                };
            }
            catch (Exception e)
            {
                return FundaApiResult.CreateErrorResult($"Request failed with error: {e.Message}");
            }
        }
    }
}
