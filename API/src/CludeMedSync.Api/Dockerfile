# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CludeMedSync/API/src/CludeMedSync.Api/CludeMedSync.Api.csproj", "CludeMedSync/API/src/CludeMedSync.Api/"]
COPY ["CludeMedSync/API/src/CludeMedSync.Data/CludeMedSync.Data.csproj", "CludeMedSync/API/src/CludeMedSync.Data/"]
COPY ["CludeMedSync/API/src/CludeMedSync.Domain/CludeMedSync.Domain.csproj", "CludeMedSync/API/src/CludeMedSync.Domain/"]
COPY ["CludeMedSync/API/src/CludeMedSync.Service/CludeMedSync.Service.csproj", "CludeMedSync/API/src/CludeMedSync.Service/"]
RUN dotnet restore "./CludeMedSync/API/src/CludeMedSync.Api/CludeMedSync.Api.csproj"
COPY . .
WORKDIR "/src/CludeMedSync/API/src/CludeMedSync.Api"
RUN dotnet build "./CludeMedSync.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CludeMedSync.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CludeMedSync.Api.dll"]