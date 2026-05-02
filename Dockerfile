FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CurrencyConverter.Web/CurrencyConverter.Web.csproj CurrencyConverter.Web/
RUN dotnet restore CurrencyConverter.Web/CurrencyConverter.Web.csproj

COPY CurrencyConverter.Web/ CurrencyConverter.Web/
WORKDIR /src/CurrencyConverter.Web
RUN dotnet publish -c Release -o /app/publish --no-restore

# Distroless: no shell, no package manager, runs as non-root (UID 1654) by default
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5125
ENV ASPNETCORE_URLS=http://0.0.0.0:5125
ENTRYPOINT ["dotnet", "CurrencyConverter.Web.dll"]
