# escape=`
FROM mcr.microsoft.com/windows/servercore:ltsc2019 AS installer
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ARG ES_URL="https://artifacts.elastic.co/downloads/elasticsearch/elasticsearch-8.6.2-windows-x86_64.zip"

RUN Write-Host "Downloading: $env:ES_ROOT_URL-$env:ES_VERSION.zip"; `
    Invoke-WebRequest ${env:ES_URL} -OutFile 'elasticsearch.zip' -UseBasicParsing; `
    Expand-Archive elasticsearch.zip -DestinationPath C:\ ; `
    Move-Item c:/elasticsearch-* /elasticsearch;


FROM mcr.microsoft.com/windows/servercore:ltsc2019
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]

ENV ES_HOME="C:\elasticsearch" `
    ES_JAVA_OPTS="-Xms1024m -Xmx1024m" `
    PATH_LOGS=c:\data\logs `
    PATH_DATA=c:\data\data

VOLUME C:\data
EXPOSE 9200 9300

WORKDIR $ES_HOME
COPY --from=installer C:\elasticsearch\ .
COPY elasticsearch.yml c:\elasticsearch\config\

SHELL ["cmd", "/S", "/C"]
CMD ".\bin\elasticsearch.bat"

HEALTHCHECK --interval=5s `
    CMD powershell -command `
    try { `
    $content = (iwr -useb http://localhost:9200/_cat/health).Content; `
    $health = $content.Split(' ')[3]; `
    if ($health -eq 'green' -or $health -eq 'yellow') { return 0 } `
    else { return 1 }; `
    } catch { return 1 }