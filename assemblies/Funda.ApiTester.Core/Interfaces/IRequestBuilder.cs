using Funda.ApiTester.Entities;
using System;

namespace Funda.ApiTester.Core.Interfaces
{
    public interface IRequestBuilder
    {
        /// <summary>
        /// Returns an URI according to the specified parameters to make a request to Funda API.
        /// </summary>
        /// <param name="region">The region to query.</param>
        /// <param name="listingType">The type of the listing.</param>
        /// <param name="contentType">The desired content type of the response.</param>
        /// <param name="page">Page number for the requested resource.</param>
        /// <param name="withGarden">Parameter to limit the query to properties with a garden.</param>
        /// <returns></returns>
        Uri BuildRequestUri(string region, ListingType listingType, ResponseContentType contentType, int page, bool withGarden = false);
    }
}
