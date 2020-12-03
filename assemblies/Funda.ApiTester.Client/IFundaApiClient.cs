using System;
using System.Threading.Tasks;

namespace Funda.ApiTester.Client
{
    public interface IFundaApiClient
    {
        /// <summary>
        /// Makes an asynchronous HTTP GET request to the provided uri and returns a <see cref="FundaApiResult"./>.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<FundaApiResult> GetAsync(Uri uri);
    }
}
