# Stage 1: Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore (layer cache optimisation)
COPY ["AnalyticsService/AnalyticsService.csproj", "AnalyticsService/"]
COPY ["ManuTrack.SharedKernel/ManuTrack.SharedKernel.csproj", "ManuTrack.SharedKernel/"]
RUN dotnet restore "./AnalyticsService/AnalyticsService.csproj"

# Copy full source and build
COPY . .
WORKDIR "/src/AnalyticsService"
RUN dotnet build "./AnalyticsService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AnalyticsService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnalyticsService.dll"]
