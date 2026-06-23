FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /PortfolioWebsite

# Copy everything
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -o out

# Build and publish a release

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /PortfolioWebsite
COPY --from=build /PortfolioWebsite/out .
ENTRYPOINT ["dotnet", "./PortfolioWebsite"]