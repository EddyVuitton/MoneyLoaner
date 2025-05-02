FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ../MoneyLoaner.Api/MoneyLoaner.Api.csproj ./MoneyLoaner.Api/
COPY ../MoneyLoaner.Domain/MoneyLoaner.Domain.csproj ./MoneyLoaner.Domain/

RUN dotnet restore "MoneyLoaner.Api/MoneyLoaner.Api.csproj"

COPY ../ ./

WORKDIR /app/MoneyLoaner.Api
RUN dotnet publish "MoneyLoaner.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MoneyLoaner.Api.dll"]