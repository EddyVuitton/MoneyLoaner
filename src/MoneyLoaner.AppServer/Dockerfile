FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ../MoneyLoaner.AppServer/MoneyLoaner.AppServer.csproj ./MoneyLoaner.AppServer/
COPY ../MoneyLoaner.WebUI/MoneyLoaner.WebUI.csproj ./MoneyLoaner.WebUI/
COPY ../MoneyLoaner.Domain/MoneyLoaner.Domain.csproj ./MoneyLoaner.Domain/

RUN dotnet restore "MoneyLoaner.AppServer/MoneyLoaner.AppServer.csproj"

COPY ../ ./

WORKDIR /app/MoneyLoaner.AppServer
RUN dotnet publish "MoneyLoaner.AppServer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MoneyLoaner.AppServer.dll"]    