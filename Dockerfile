FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HotSearch.Host/HotSearch.Host.csproj", "src/HotSearch.Host/"]
COPY ["src/HotSearch.Shared/HotSearch.Shared.csproj", "src/HotSearch.Shared/"]
COPY ["src/HotSearch.Domain/HotSearch.Domain.csproj", "src/HotSearch.Domain/"]
COPY ["src/Segmenter/Segmenter.csproj", "src/Segmenter/"]
COPY ["nuget.config", "nuget.config"]
RUN dotnet restore "./src/HotSearch.Host/HotSearch.Host.csproj"
COPY . .
WORKDIR "/src/src/HotSearch.Host"
RUN dotnet build "./HotSearch.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HotSearch.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotSearch.Host.dll"]