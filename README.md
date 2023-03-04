![Hackathon Logo](docs/images/hackathon.png?raw=true "Hackathon Logo")
# Sitecore Hackathon 2023

- MUST READ: **[Submission requirements](SUBMISSION_REQUIREMENTS.md)**
- [Entry form template](ENTRYFORM.md)

### ⟹ [Insert your documentation here](ENTRYFORM.md) <<

## TODO: merge with ENTRYFORM instructions ...

### Getting started

1. Run `.\Init.ps1 -LicenseXmlPath "<C:\path\to\license.xml>"`
1. Run `dotnet tool restore`
1. Run `msbuild D:\git\2023-Team-NotImplementedException /t:"Restore;Build;WebPublish" /p:DeployOnBuild=true /p:PublishProfile=Local` or open Visual Studio solution and publish the `Platform` project.
1. Run `.\Up.ps1`, opens <https://xmcloudcm.localhost/sitecore/>
1. Run `curl.exe -k https://xmcloudcm.localhost/layouts/InitializeWebhooks.aspx` (unfortunately Sitecore Webhooks *only* initializes during startup or item:save and NOT when pushing serialized items)
