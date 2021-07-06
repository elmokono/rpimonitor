FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["wwwroot/homeMonitor.csproj", "wwwroot/"]
RUN dotnet restore "wwwroot/homeMonitor.csproj"
COPY . .
WORKDIR "/src/wwwroot"
RUN dotnet build "homeMonitor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "homeMonitor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "homeMonitor.dll"]
