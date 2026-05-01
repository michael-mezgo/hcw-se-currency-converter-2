using System.Globalization;
using System.Xml.Linq;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CurrencyConverter.Web
{
    public class CurrencyConverterService : CurrencyService.CurrencyServiceBase
    {
        private readonly string ECB_URL = Environment.GetEnvironmentVariable("ECB_URL") ?? throw new Exception("ECB_URL not set");
        private readonly string VALID_KEY = Environment.GetEnvironmentVariable("API_KEY") ?? throw new Exception("API_KEY not set");

        public override Task<ConvertResponse> Convert(ConvertRequest request, ServerCallContext context)
        {
            if (request.ApiKey != VALID_KEY)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Authentication failed"));

            var rates = LoadRates();

            double fromRate = request.FromCurrency == "EUR" ? 1 : rates[request.FromCurrency];
            double toRate = request.ToCurrency == "EUR" ? 1 : rates[request.ToCurrency];

            double result = (request.Amount / fromRate) * toRate;

            return Task.FromResult(new ConvertResponse { Result = result });
        }

        public override Task<SupportedCurrenciesResponse> GetSupportedCurrencies(Empty request, ServerCallContext context)
        {
            var rates = LoadRates();
            var currencies = rates.Keys.ToList();
            currencies.Add("EUR");
            currencies.Sort();

            var response = new SupportedCurrenciesResponse();
            response.Currencies.AddRange(currencies);
            return Task.FromResult(response);
        }

        private Dictionary<string, double> LoadRates()
        {
            var xml = XDocument.Load(ECB_URL);

            return xml.Descendants()
                .Where(x => x.Name.LocalName == "Cube" && x.Attribute("currency") != null)
                .ToDictionary(
                    x => x.Attribute("currency")!.Value,
                    x => double.Parse(x.Attribute("rate")!.Value, CultureInfo.InvariantCulture)
                );
        }
    }
}
