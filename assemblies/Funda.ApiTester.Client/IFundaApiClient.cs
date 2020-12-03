using System;
using System.Threading.Tasks;

namespace Funda.ApiTester.Client
{
    public interface IFundaApiClient
    {
        Task<FundaApiResult> GetAsync(Uri uri);
    }
}
