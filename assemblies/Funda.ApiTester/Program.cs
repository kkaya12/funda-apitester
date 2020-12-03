using Funda.ApiTester.Client;
using Funda.ApiTester.Core;
using Funda.ApiTester.Core.Interfaces;
using Funda.ApiTester.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funda.ApiTester
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var responseContentType = configuration.GetSection("Response")["ContentType"];

            // TODO: Add logging.
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            serviceCollection.AddSingleton<IFundaApiClient, FundaApiHttpClient>();
            serviceCollection.AddSingleton<IRequestBuilder, RequestBuilder>();
            serviceCollection.AddSingleton<IStaticRequestParametersProvider, StaticRequestParametersProvider>();
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            //TODO: Add factory/strategy to select concrete implementation.
            if (responseContentType.Equals("json", StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrWhiteSpace(args[0]))
            {
                serviceCollection.AddSingleton<IFundaApiTester, FundaApiTesterJson>();
            }
            else if (responseContentType.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                serviceCollection.AddSingleton<IFundaApiTester, FundaApiTesterXml>();
            }
            else
            {
                Console.WriteLine($"Response content type {responseContentType} is invalid. Press any key to exit.");
                Console.ReadKey();
                return;
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var region = configuration.GetSection("Request")?["Region"];
            if (string.IsNullOrWhiteSpace(region))
            {
                Console.WriteLine($"Please provide a region and run again.");
                Console.ReadKey();
                return;
            }

            if (!bool.TryParse(configuration.GetSection("Request")?["WithGarden"], out var withGarden))
            {
                Console.WriteLine($"Setting with garden is invalid. Default setting {false} will be used.");
                withGarden = false;
            }

            if (!int.TryParse(configuration.GetSection("Response")?["Limit"], out var limit) || limit <= 0)
            {
                Console.WriteLine($"Limit {limit} is invalid. Press any key to exit.");
                Console.ReadKey();
                return;
            }

            Task<IDictionary<RealEstateAgent, int>> requestTask;
            if (withGarden)
            {
                requestTask = serviceProvider.GetService<IFundaApiTester>().GetTopXAgentsByNumberOFListingsForSaleWithGardenAsync(limit, region);
            }
            else
            {
                requestTask = serviceProvider.GetService<IFundaApiTester>().GetTopXAgentsByNumberOFListingsForSaleAsync(limit, region);
            }

            requestTask.Wait();
            if (!requestTask.IsCompleted)
            {
                Console.WriteLine("Request cannot be completed within timeout of 30 seconds.");
                return;
            }

            var result = requestTask.Result;

            if (result == null || !result.Any())
            {
                Console.Write($"No results to show, press any key to exit.");
                Console.ReadKey();
                return;
            }

            var i = 1;
            Console.WriteLine($"-- Parameters: Region: {region}, With garden: {withGarden} - Top {limit}--");
            foreach(var item in result)
            {
                Console.WriteLine($"{i}. Real estate agent {item.Key}, #Listings: {item.Value}");
                i++;
            }

            Console.WriteLine("\n Press any key to exit.");
            Console.ReadKey();
        }
    }
}
