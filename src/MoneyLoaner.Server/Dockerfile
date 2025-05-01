FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "MoneyLoaner.Server/MoneyLoaner.Server.csproj"
WORKDIR "/src/MoneyLoaner.Server"
RUN dotnet build "MoneyLoaner.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoneyLoaner.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoneyLoaner.Server.dll"]