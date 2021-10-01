FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY ["./src/Stocks.Api/Stocks.Api.csproj", "./Stocks.Api/"]
COPY ["./src/Stocks.Application/Stocks.Application.csproj", "./Stocks.Application/"]
COPY ["./src/Stocks.Domain/Stocks.Domain.csproj", "./Stocks.Domain/"]
COPY ["./src/Stocks.Infrastructure/Stocks.Infrastructure.csproj", "./Stocks.Infrastructure/"]
RUN dotnet restore "./Stocks.Api/Stocks.Api.csproj"

COPY /src .
RUN dotnet build -c Release "./Stocks.Api/Stocks.Api.csproj"

FROM build AS publish
RUN dotnet publish "./Stocks.Api/Stocks.Api.csproj" -c Release -o /out --no-restore --no-build

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
EXPOSE 5000
COPY --from=publish /out ./

ENTRYPOINT ["dotnet", "Stocks.Api.dll"]