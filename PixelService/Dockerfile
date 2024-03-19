# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o out

# Stage 2: Publish and Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /src/out/ .
ENTRYPOINT ["dotnet", "PixelService.dll"]