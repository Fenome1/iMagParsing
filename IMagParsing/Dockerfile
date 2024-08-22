FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IMagParsing/IMagParsing.csproj", "IMagParsing/"]
COPY ["IMagParsing.Infrastructure/IMagParsing.Infrastructure.csproj", "IMagParsing.Infrastructure/"]
COPY ["IMagParsing.Core/IMagParsing.Core.csproj", "IMagParsing.Core/"]
RUN dotnet restore "IMagParsing/IMagParsing.csproj"
COPY . .
WORKDIR "/src/IMagParsing"
RUN dotnet build "IMagParsing.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IMagParsing.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IMagParsing.dll"]
