# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY wwwroot/*.sln ./aspnetapp/
COPY wwwroot/*.csproj ./aspnetapp/
WORKDIR /source/aspnetapp
RUN dotnet restore

# copy everything else and build app
COPY wwwroot/. ./aspnetapp/
WORKDIR /source/aspnetapp
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "homeMonitor.dll"]