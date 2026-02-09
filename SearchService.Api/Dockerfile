# ---- Base image for runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# ---- SDK image for building ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only project files first (for faster restore)
COPY SearchService.Api/*.csproj ./SearchService.Api/
COPY SearchService.Contracts/*.csproj ./SearchService.Contracts/

# Restore dependencies
RUN dotnet restore ./SearchService.Api/SearchService.Api.csproj

# Copy everything else
COPY . .

# Build
WORKDIR /src/SearchService.Api
RUN dotnet build ./SearchService.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish ./SearchService.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---- Final runtime image ----
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SearchService.Api.dll"]
