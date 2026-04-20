# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat


# Base runtime image (Linux)
FROM mcr.microsoft.com/dotnet/aspnet:10.0.2 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Build stage (Linux SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0.201 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project file and restore
COPY ["GpsApiWorker/GpsApiWorker.csproj", "GpsApiWorker/"]
COPY ["ArmsModels/Core.csproj", "ArmsModels/"]
COPY ["ArmsServices/DAL.csproj", "ArmsServices/"]
RUN dotnet restore "./GpsApiWorker/GpsApiWorker.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/GpsApiWorker"
RUN dotnet build "./GpsApiWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GpsApiWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GpsApiWorker.dll"]