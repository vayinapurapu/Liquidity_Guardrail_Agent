# FunctionApp

Lightweight Azure Functions app that aggregates daily statement data and exposes two HTTP endpoints:

- `GET /api/aggregate` — aggregates CSV/XLSX files from a local `data` folder and returns a summary and recent details.
- `GET /api/aggregatesp` — same aggregation but downloads files from SharePoint (Graph) before aggregating.

## Prerequisites

- .NET 8 SDK
- Azure Functions Core Tools (for local development and publish)
- (For `aggregatesp`) an Azure AD app with client credentials that can access Microsoft Graph

## Environment variables

- `DATA_FOLDER` (optional) — local data folder path. Defaults to `D:\home\site\wwwroot\data` on Windows or `/home/site/wwwroot/data` on Linux.
- `SP_CLIENT_ID` — SharePoint / AAD application client id (for `aggregatesp`).
- `SP_TENANT_ID` — AAD tenant id.
- `SP_CLIENT_SECRET` — Client secret for the AAD app.
- `SP_HOSTNAME` — SharePoint host (e.g. `contoso.sharepoint.com`).
- `SP_SITE_PATH` — Site path (e.g. `/sites/mysite`).
- `SP_FOLDER_PATH` (optional) — path inside the drive to look for files.

Make sure these are provided in local.settings.json for local runs, or as App Settings when deployed to Azure Functions.

## Build and run locally

From the `FunctionApp` folder:

```powershell
dotnet build
func start
```

The Functions host will start (default port 7071). Endpoints:

- `http://localhost:7071/api/aggregate`
- `http://localhost:7071/api/aggregatesp`

Example call (PowerShell):

```powershell
Invoke-RestMethod -Uri "http://localhost:7071/api/aggregate?asOf=2026-03-17&lookbackDays=7"
```

## Publish

Publish directly with the Functions Core Tools:

```powershell
func azure functionapp publish <FUNCTION_APP_NAME>
```

Or publish via `dotnet` then deploy using your preferred method:

```powershell
dotnet publish -c Release
# then deploy the generated artifacts from bin/Release/net8.0/publish
```

## Notes & troubleshooting

- `aggregate` reads CSV/XLSX files using `DataHelpers.LoadStatements` from the configured `DATA_FOLDER`.
- `aggregatesp` will download supported files (`.csv`, `.xlsx`) from the configured SharePoint site into a cache folder under the `DATA_FOLDER` before processing.
- If `aggregatesp` returns a 400, verify the `SP_*` environment variables are set.
- Logs are written via the Functions logger; use the Functions host logs or Application Insights when deployed.

## Files of interest

- Aggregation logic: `AggregationFunction.cs`
- Helpers & models: `Helpers.cs`, `Models.cs`

If you'd like, I can also add a sample `local.settings.json` template and a tiny example dataset in `data/`.
