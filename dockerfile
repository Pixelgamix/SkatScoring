FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY SkatScoring.DataAccess/*.csproj ./SkatScoring.DataAccess/
COPY SkatScoring.Contracts/*.csproj ./SkatScoring.Contracts/
COPY SkatScoring.WebApi/*.csproj ./SkatScoring.WebApi/

WORKDIR /app/SkatScoring.WebApi
RUN dotnet restore

# Copy everything else and build
WORKDIR /app
COPY SkatScoring.DataAccess/. ./SkatScoring.DataAccess/
COPY SkatScoring.Contracts/. ./SkatScoring.Contracts/
COPY SkatScoring.WebApi/. ./SkatScoring.WebApi/

WORKDIR /app/SkatScoring.WebApi
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/SkatScoring.WebApi/out .
ENTRYPOINT ["dotnet", "SkatScoring.WebApi.dll"]
