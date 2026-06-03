# Agent Academy Hackathon Idea

Problem statement
-----------------

Teams often store daily financial statements (CSV/XLSX) across SharePoint sites and folders. Manually locating, downloading, and aggregating those files is time-consuming and error-prone, and there is limited automated detection for balance breaches or unusual variance across business units and banks.

Use case
--------

- Treasury and finance operations need a lightweight, automated service that aggregates daily statements, computes short-term rolling averages, highlights variance and threshold breaches, and exposes results as JSON for dashboards or alerting.
- The service must support local file processing (for dev or local runs) and pulling files from SharePoint (for production workflows) using service credentials.

Approach
--------

- Implemented as an Azure Functions app with two HTTP endpoints:
	- `GET /api/aggregate` — reads files from a local `data` folder and returns aggregated summary and recent details.
	- `GET /api/aggregatesp` — uses Microsoft Graph (client credentials) to download supported files from a configured SharePoint site into a local cache, then aggregates them.
- Processing pipeline:
	1. Load statement rows from CSV/XLSX using `DataHelpers.LoadStatements`.
	2. Select an "as-of" date and a lookback window (default 7 days).
	3. Compute 7-day rolling averages per (Business Unit, Bank, Currency) and compare today's closing balances against averages.
	4. Apply simple currency thresholds and mark breaches.
	5. Return a compact JSON payload with `AsOf`, `Summary`, and `Details` for downstream consumers.

Architecture & files
--------------------

- Aggregation logic: [FunctionApp/AggregationFunction.cs](FunctionApp/AggregationFunction.cs)
- Helpers & models: [FunctionApp/Helpers.cs](FunctionApp/Helpers.cs) and [FunctionApp/Models.cs](FunctionApp/Models.cs)
- Local data folder: `data/` (configurable via `DATA_FOLDER` environment variable)

Quick start (local)
-------------------

From the `FunctionApp` folder, build and start the Functions host:

```powershell
dotnet build
func start
```

Required environment variables (set in `local.settings.json` or App Settings):

- `DATA_FOLDER` (optional) — local data path, defaults to `D:\home\site\wwwroot\data` on Windows or `/home/site/wwwroot/data` on Linux.
- `SP_CLIENT_ID`, `SP_TENANT_ID`, `SP_CLIENT_SECRET`, `SP_HOSTNAME`, `SP_SITE_PATH` — required for `aggregatesp`.

Example request:

```powershell
Invoke-RestMethod -Uri "http://localhost:7071/api/aggregate?asOf=2026-03-17&lookbackDays=7"
```

Next steps
----------

- Add a `local.settings.json` template and a small example dataset in `FunctionApp/data/` for easier onboarding.
- Add unit tests around `DataHelpers.LoadStatements` and aggregation logic.
- Add optional alerting (email/Teams) when threshold breaches are detected.

