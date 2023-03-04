# escape=`

ARG BUILD_IMAGE

# ---
FROM ${BUILD_IMAGE} AS nuget-prep
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]
COPY *.sln nuget.config Directory.Packages.props /nuget/
COPY src/ /temp/
RUN Invoke-Expression 'robocopy C:/temp C:/nuget/src /s /ndl /njh /njs *.csproj'

# ---
FROM ${BUILD_IMAGE} AS builder
ARG BUILD_CONFIGURATION

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

WORKDIR /build

COPY --from=nuget-prep ./nuget ./
RUN nuget restore -Verbosity quiet

COPY src/ ./src/

WORKDIR /build/src/platform
RUN msbuild .\Platform.csproj /t:'Restore;Build;WebPublish' /p:Configuration=$env:BUILD_CONFIGURATION /p:LangVersion=11.0 /m /p:DeployOnBuild=true /p:PublishProfile=Local /v:m

# ---
FROM mcr.microsoft.com/windows/nanoserver:1809
WORKDIR /artifacts
COPY --from=builder /build/docker/deploy/platform ./sitecore/
