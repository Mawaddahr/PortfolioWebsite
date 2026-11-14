FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
# .NET 8+ luistert in containers standaard op poort 8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./PortfolioWebsite.csproj", "./"]
RUN dotnet restore "./PortfolioWebsite.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "PortfolioWebsite.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PortfolioWebsite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Draai niet als root (lokale uploads mogen nog wel naar wwwroot/imguploads)
RUN mkdir -p /app/wwwroot/imguploads && chown -R $APP_UID /app/wwwroot
USER $APP_UID

# De omgeving (Production/Development) komt uit ASPNETCORE_ENVIRONMENT; standaard Production.
ENTRYPOINT ["dotnet", "PortfolioWebsite.dll"]
