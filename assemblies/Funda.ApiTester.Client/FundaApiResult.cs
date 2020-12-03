using System.Net;

namespace Funda.ApiTester.Client
{
    public class FundaApiResult
    {
        public bool IsSuccesss { get; set; }
        public string Content { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static FundaApiResult CreateErrorResult(string message, HttpStatusCode statusCode)
        {
            return new FundaApiResult
            {
                IsSuccesss = false,
                Content = string.Empty,
                ErrorMessage = message,
                StatusCode = statusCode
            };
        }

        public static FundaApiResult CreateErrorResult(string message)
        {
            return new FundaApiResult
            {
                IsSuccesss = false,
                Content = string.Empty,
                ErrorMessage = message
            };
        }
    }
}
