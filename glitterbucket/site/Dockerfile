ARG BUILD_IMAGE=mcr.microsoft.com/dotnet/sdk:6.0
ARG RUN_IMAGE=mcr.microsoft.com/dotnet/aspnet:6.0

FROM ${BUILD_IMAGE} AS base

WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY code/*.csproj ./code/
RUN dotnet restore

# copy everything else and build app
COPY code/ ./code/
WORKDIR /source/code

FROM base as watch

ENV DOTNET_USE_POLLING_FILE_WATCHER=1 \
    DOTNET_WATCH_SUPPRESS_LAUNCH_BROWSER=1 \
    ASPNETCORE_URLS=http://*:3000

ENTRYPOINT ["dotnet", "watch", "--", "run", "--no-launch-profile", "--no-restore", "-c", "Debug"]

FROM base as build
WORKDIR /source/code
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c %BUILD_CONFIGURATION% -o /app --no-restore


# final stage/image
FROM ${RUN_IMAGE}
EXPOSE 3000
ENV ASPNETCORE_URLS=http://*:3000
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "glitterbucket.dll"]
