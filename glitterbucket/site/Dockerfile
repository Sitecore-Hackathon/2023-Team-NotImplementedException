ARG BUILD_IMAGE=mcr.microsoft.com/dotnet/sdk:6.0
ARG RUN_IMAGE=mcr.microsoft.com/dotnet/aspnet:6.0

FROM ${BUILD_IMAGE} AS build

WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY code/*.csproj ./code/
RUN dotnet restore

# copy everything else and build app
COPY code/ ./code/
WORKDIR /source/code
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM ${RUN_IMAGE}
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "glitterbucket.dll"]
