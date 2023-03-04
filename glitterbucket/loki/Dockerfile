ARG BUILD_IMAGE=mcr.microsoft.com/windows/servercore:ltsc2022
ARG RUN_IMAGE=mcr.microsoft.com/windows/nanoserver:ltsc2022

FROM ${BUILD_IMAGE} AS builder

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ARG LOKI_URL="https://github.com/grafana/loki/releases/download/v2.7.4/loki-windows-amd64.exe.zip"
RUN [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; \
    Invoke-WebRequest -Uri ${env:LOKI_URL} -UseBasicParsing -OutFile 'loki.zip' ; \
    New-Item '/loki' -ItemType Directory | Out-Null ; \
    Expand-Archive 'loki.zip' -Destination '/loki' ; \
    Remove-Item 'loki.zip' ;

COPY *.yml /loki

FROM ${RUN_IMAGE}
COPY --from=builder /loki /loki
ENTRYPOINT ["/loki/loki-windows-amd64.exe"]
CMD ["-config.file=/loki/loki-config.yml"]
