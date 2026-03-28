namespace CurrencyConverter.Web
{
    using System.ServiceModel;

    [ServiceContract]
    public interface ICurrencyService
    {
        [OperationContract]
        double Convert(string fromCurrency, string toCurrency, double amount, string apiKey);
    }
}
