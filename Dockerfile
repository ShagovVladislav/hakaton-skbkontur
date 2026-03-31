FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MockApi.Presentation/MockApi.Presentation.csproj", "MockApi.Presentation/"]
COPY ["MockApi.Application/MockApi.Application.csproj", "MockApi.Application/"]
COPY ["MockApi.Infrastructure/MockApi.Infrastructure.csproj", "MockApi.Infrastructure/"]
COPY ["MockApi.Domain/MockApi.Domain.csproj", "MockApi.Domain/"]
RUN dotnet restore "MockApi.Presentation/MockApi.Presentation.csproj"
COPY . .
WORKDIR "/src/MockApi.Presentation"
RUN dotnet build "./MockApi.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MockApi.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockApi.Presentation.dll"]
