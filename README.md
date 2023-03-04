![Hackathon Logo](docs/images/hackathon.png?raw=true "Hackathon Logo")
# Sitecore Hackathon 2023

- MUST READ: **[Submission requirements](SUBMISSION_REQUIREMENTS.md)**
- [Entry form template](ENTRYFORM.md)

## Team name

Team NotImplementedException

## Category

Best Enhancement to XM Cloud

## Description

### Module Purpose

**... TODO ...**

### What problem was solved

**... TODO ...**

### How does this module solve it

**... TODO ...**

## Video link

**... TODO ...**

## Pre-requisites and Dependencies

- Windows 10/11
- Some Docker engine, for example [Docker Desktop](https://desktop.docker.com/win/stable/amd64/Docker%20Desktop%20Installer.exe)
- Visual Studio 2022

## Installation instructions

1. Run `dotnet tool restore`
1. Run `.\glitteraudit\Init.ps1 -LicenseXmlPath "<C:\path\to\license.xml>"`
1. Run `msbuild .\glitteraudit\ /t:"Restore;Build;WebPublish" /p:DeployOnBuild=true /p:PublishProfile=Local` or open Visual Studio solution and publish the `Platform` project.
1. Run `.\Up.ps1`

### Configuration

**... TODO ...**

## Usage instructions

**... TODO ...**

## Comments

**... TODO ...**

- Unfortunately Sitecore Webhooks *only* initializes during startup or item:save and NOT when pushing serialized items with `dotnet sitecore ser push`. As a workaround `.\Up.ps1` does a http call to `./glitteraudit/src/platform/layouts/InitializeWebhooks.aspx` that does the initialization.
