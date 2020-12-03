using Funda.ApiTester.Core.Interfaces;
using Funda.ApiTester.Entities;
using System;
using System.Text;

namespace Funda.ApiTester.Core
{

    public class RequestBuilder : IRequestBuilder
    {
        private string _addressBase;
        private string _apiKey;

        public RequestBuilder(IStaticRequestParametersProvider requestParametersProvider)
        {
            _apiKey = requestParametersProvider.GetKey() ?? throw new ArgumentNullException(nameof(requestParametersProvider));
            _addressBase = requestParametersProvider.GetAddressBase();
        }

        public Uri BuildRequestUri(string region, ListingType listingType, ResponseContentType contentType, int page, bool withGarden = false)
        {
            if (string.IsNullOrWhiteSpace(region)) return null;
            var addressBuilder = new StringBuilder(_addressBase);

            switch (contentType)
            {
                case ResponseContentType.Json:
                    addressBuilder.Append("json/");
                    break;
                case ResponseContentType.Xml:
                    addressBuilder.Append("xml/");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            addressBuilder.Append($"{_apiKey}/?type=");

            switch (listingType)
            {
                case ListingType.Rent:
                    addressBuilder.Append("huur&zo=/");
                    break;
                case ListingType.Sale:
                    addressBuilder.Append("koop&zo=/");
                    break;
                default:
                    break;
            }

            addressBuilder.Append($"{region}/");

            if (withGarden)
            {
                addressBuilder.Append("tuin/");
            }

            addressBuilder.Append($"&page={page}&pagesize=25");

            return new Uri(addressBuilder.ToString());
        }
    }
}
