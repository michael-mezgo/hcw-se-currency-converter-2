using CurrencyConverter.Web;
using SoapCore;
using System.ServiceModel;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();
builder.Services.AddSingleton<ICurrencyService, CurrencyService>();
var app = builder.Build();

app.UseRouting();
app.UseSoapEndpoint<ICurrencyService>(
    "/CurrencyService.svc",
    new SoapEncoderOptions()
);

app.Run();