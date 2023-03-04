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

### Setup (once)

1. Run `.\glitterxmc\Init.ps1 -LicenseXmlPath "<C:\path\to\license.xml>"`
1. Run `dotnet tool restore`
1. Run `dotnet sitecore cloud login`
1. Run `dotnet sitecore connect --ref xmcloud --cm https://xmcloudcm.localhost --allow-write true -n default`

### Startup

1. Run `docker compose up -d --build`
1. Run `dotnet sitecore index schema-populate`
1. Run `dotnet sitecore ser push`
1. Run `curl.exe -k https://xmcloudcm.localhost/layouts/InitializeWebhooks.aspx` (unfortunately Sitecore webhooks *only* initializes during startup OR item:save on a handler but NOT when pushing serialized items... \*sigh\*)
1. Run `Start-Process https://xmcloudcm.localhost/sitecore/`

### Shutdown

1. Run `docker compose down`

### Configuration

**... TODO ...**

## Usage instructions

**... TODO ...**

## Comments

**... TODO ...**
