# escape=`
FROM mcr.microsoft.com/windows/servercore:ltsc2019 AS downloader
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ARG KIBANA_URI="https://artifacts.elastic.co/downloads/kibana/kibana-8.6.2-windows-x86_64.zip"

RUN Invoke-WebRequest "${env:KIBANA_URI}" -OutFile 'kibana.zip' -UseBasicParsing; `
    Expand-Archive kibana.zip -DestinationPath C:\; `
    Move-Item c:/kibana-* /kibana

# Kibana
FROM mcr.microsoft.com/windows/servercore:ltsc2019


EXPOSE 5601
WORKDIR C:\kibana
COPY --from=downloader C:\kibana\ .
COPY kibana.yml c:\kibana\conf\kibana.yml

ENV KIBANA_HOME="C:\kibana"

SHELL ["cmd", "/S", "/C"]
CMD ".\bin\kibana.bat" serve -H 0.0.0.0 -e http://elasticsearch:9200

#HEALTHCHECK --start-period=30s --interval=10s --retries=5 `
#    CMD powershell -command `
#    try { `
#    $response = iwr -useb http://localhost:5601/app/kibana; `
#    if ($response.StatusCode -eq 200) { return 0} `
#    else {return 1}; `
#    } catch { return 1 }
#