using CurrencyConverter.Web;
using Microsoft.AspNetCore.Server.Kestrel.Core;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT")
    ?? throw new Exception("PORT not set");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port), o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<CurrencyConverterService>();

app.Run();