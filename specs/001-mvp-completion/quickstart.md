# Validate PetCare Hub

Use the .NET 10 SDK and PowerShell. From the repo root run:

```powershell
dotnet build PetCareHub.sln
pwsh -NoProfile -File scripts/smoke-mvp.ps1
```

The smoke script starts both apps on loopback using a temporary SQLite database. It checks dashboard access, pet creation and details, reminder creation and completion, rejected writes, and persistence after restarting the API.

For a manual walkthrough follow the [README](../../README.md). Create a pet, open its details and add a Checkup due in 30 days. Verify it appears under Reminders and stays outside the Dashboard upcoming window. Edit its due date to tomorrow and confirm it enters the upcoming results. Mark it done and confirm it disappears from the pending list. Resize to a narrow viewport and verify the three navigation links remain accessible. See the [HTTP contract](contracts/http.md) for expected API behavior.
