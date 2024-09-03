FROM hub.lumigame.com/base/dotnet/core/sdk:8.0 AS base
FROM hub.lumigame.com/base/dotnet/core/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.Schedule.Service/Tgs.Schedule.Service.csproj", "Tgs.Schedule.Service/"]
RUN dotnet restore "./Tgs.Schedule.Service/./Tgs.Schedule.Service.csproj"
COPY . .
WORKDIR "/src/Tgs.Schedule.Service"
RUN dotnet build "./Tgs.Schedule.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.Schedule.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.Schedule.Service.dll"]