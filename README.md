![Hackathon Logo](docs/images/hackathon.png?raw=true "Hackathon Logo")
# Sitecore Hackathon 2023

- MUST READ: **[Submission requirements](SUBMISSION_REQUIREMENTS.md)**
- [Entry form template](ENTRYFORM.md)

## Team name

Team NotImplementedException

## Category

Best Enhancement to XM Cloud

## Description

**Glitter Audit**, your goto XM Cloud tool for everything content auditing!

### Module Purpose

Collects information on item, version and field changes to surface basic audit and historic information for Sitecore users *in context* of their current editor (ie. Content Editor, Experience Editor or Pages) using a browser extension. All data is stored in Elasticsearch and can be further queried and analyzed in great details.

We also want to show how to do integrations in Sitecore using webhooks, keeping functionality in separate and isolated processes, perfect fit for XM Cloud but also a good approach for classic Sitecore 10.3 deployments.

### What problem was solved

Users of Sitecore often wants an easy way to see "what happened to an item" and the OOB audit log is not *easy* accessible to the common user and does not contain much needed information such as *field value changes*.

### How does this module solve it

A standard Sitecore 10.3 / XM Cloud [webhook event handler](https://doc.sitecore.com/xp/en/developers/103/sitecore-experience-manager/webhooks.html) is configured to trigger on all events [available](https://doc.sitecore.com/xp/en/developers/103/sitecore-experience-manager/webhook-event-handler-configuration-fields.html#supported-events) and posts event data to our web app "glitterbucket" that persists the data in Elasticsearch. The "glitterbucket" web app also exposes data endpoints for the browser extension "glitterchromiumextension" to show the UI. Developers/administrators can also use Kibana to query the data directly.

![Solution overview](docs/images/overview.png?raw=true "Solution overview")

## Video link

DAMN!

## Pre-requisites and dependencies

- Windows 10/11
- Some Docker engine, for example [Docker Desktop](https://desktop.docker.com/win/stable/amd64/Docker%20Desktop%20Installer.exe)

## Installation instructions

### Setup (once)

#### Initialize solution

1. Run `.\glitterxmc\Init.ps1 -LicenseXmlPath "<C:\path\to\license.xml>"`
1. Run `dotnet tool restore`
1. Run `dotnet sitecore cloud login`
1. Run `dotnet sitecore connect --ref xmcloud --cm https://xmcloudcm.localhost --allow-write true -n default`

#### Install browser extension

1. In the address bar type `edge://extensions/` (Microsoft Edge) or `chrome://extensions/` on Google Chrome
1. In the extension bar, enable `Developer Mode`
1. Click `Load unpacked` and point to the folder `./glitterchromiumextension`
1. Ensure the extension is visible by clicking the "Show in toolbar" in the extensions menu.

### Startup

1. Run `docker-compose up -d --build`
1. Run `dotnet sitecore index schema-populate`
1. Run `dotnet sitecore ser push`
1. Run `curl.exe -k https://xmcloudcm.localhost/layouts/InitializeWebhooks.aspx` (unfortunately Sitecore webhooks *only* initializes during startup OR item:save on a handler but NOT when pushing serialized items... \*sigh\*)

## Configuration

1. Open <https://kibana.localhost/app/management/kibana/dataViews>
1. Click "Create data view"
    - Name: "test"
    - Index pattern: "glitteraudit-*"
    - Timestamp field: "timestamp"
1. Open <https://kibana.localhost/app/discover>

## Usage instructions

Open up <https://xmcloudcm.localhost/sitecore/> in Microsoft Edge or Google Chrome, create some new items, edit some, create new versions and click the extension to peek the data generated.

For advanced querying, open Kibana at <https://kibana.localhost/>
