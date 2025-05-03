FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ../MoneyLoaner.App/MoneyLoaner.App.csproj ./MoneyLoaner.App/
COPY ../MoneyLoaner.WebUI/MoneyLoaner.WebUI.csproj ./MoneyLoaner.WebUI/
COPY ../MoneyLoaner.Domain/MoneyLoaner.Domain.csproj ./MoneyLoaner.Domain/

RUN dotnet restore "MoneyLoaner.App/MoneyLoaner.App.csproj"

COPY ../ ./

WORKDIR /app/MoneyLoaner.App
RUN dotnet publish "MoneyLoaner.App.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MoneyLoaner.App.dll"]    