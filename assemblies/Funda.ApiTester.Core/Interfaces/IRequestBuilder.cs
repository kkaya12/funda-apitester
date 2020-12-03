using Funda.ApiTester.Entities;
using System;

namespace Funda.ApiTester.Core.Interfaces
{
    public interface IRequestBuilder
    {
        Uri BuildRequestUri(string region, AdType adType, ResponseContentType contentType, int page, bool withGarden = false);
    }
}
