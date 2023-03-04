ARG BUILD_IMAGE=mcr.microsoft.com/windows/servercore:ltsc2022

FROM ${BUILD_IMAGE}

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ARG GRAFANA_URI="https://dl.grafana.com/oss/release/grafana-9.4.3.windows-amd64.zip"

RUN [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; \
    Invoke-WebRequest -Uri ${env:GRAFANA_URI} -UseBasicParsing -OutFile 'grafana.zip' ; \
    Expand-Archive 'grafana.zip' -Destination '/' ; \
    Remove-Item 'grafana.zip' ; \
    Move-Item grafana* grafana ; \
    Remove-Item /grafana/conf/provisioning/dashboards/* ; 

COPY custom.ini /grafana/conf/

ENTRYPOINT "c:\grafana\bin\grafana-server.exe" --homepath=c:\\grafana --packaging=docker
