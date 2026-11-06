# PetCare Hub – MVP

Simple and clean .NET project for learning backend design patterns.

## Design Patterns Used

| Pattern              | Where to find it                                      |
|----------------------|-------------------------------------------------------|
| Repository           | `Database/Repositories`                               |
| Factory              | `PetFactory`, `ReminderStrategyFactory`               |
| Builder              | `PetBuilder`                                          |
| Strategy             | `IReminderStrategy` + concrete strategies             |
| Observer             | `IDomainEvent` + `IEventDispatcher`                   |
| Singleton            | `PetFactory`, `ReminderStrategyFactory` registration  |
| Adapter              | `INotificationAdapter` + `FakeNotificationAdapter`    |
| Decorator            | Logging middleware in `Program.cs`                    |
| CQRS                 | Separate Command / Query methods in services          |
| Circuit Breaker      | `CircuitBreakerNotificationAdapter`                   |

## Solution Structure

```
PetCareHub/
├── PetCareHub.Api          → REST API + Swagger
├── PetCareHub.WebApp       → Tailwind UI (calls API)
├── PetCareHub.Domain       → Feature services + patterns
├── PetCareHub.Database     → EF Core + Repositories
└── PetCareHub.Shared       → Result pattern
```

## How to Run

### Requirements
- .NET 10 SDK

### Steps

1. **Start the API** (Terminal 1):
```bash
cd PetCareHub.Api
dotnet restore
dotnet run
```
Check the console for the port (e.g. `http://localhost:5280`).

2. **Update API URL** (if different port):
Open `PetCareHub.WebApp/appsettings.json` and set:
```json
"ApiSettings": {
  "BaseUrl": "http://localhost:YOUR_PORT/"
}
```

3. **Start the WebApp** (Terminal 2):
```bash
cd PetCareHub.WebApp
dotnet restore
dotnet run
```

4. Open the WebApp URL shown in the console (usually `http://localhost:5xxx`).

## Features in UI

- **Dashboard** – pets count + upcoming reminders
- **Pets** – list, create, view details, and edit profiles
- **Reminders** – create, edit and complete reminders from the list of all pending tasks. The Dashboard shows overdue tasks and tasks due through the next 14 days.

## UI

- Tailwind CSS (CDN)
- Clean cards, forms, and navigation
- Responsive layout

## Demo User

API uses a fixed demo user ID. No login required for MVP.

To verify the MVP, run `dotnet build PetCareHub.sln`, then run `pwsh -NoProfile -File scripts/smoke-mvp.ps1` from the repository root. The smoke test uses a temporary SQLite database.
