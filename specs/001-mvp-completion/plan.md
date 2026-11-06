# Implementation Plan | PetCare Hub MVP completion

**Branch** — `master` · **Date** — 2026-11-06 · [Specification](spec.md)

## Summary

Finish the documented dashboard, pet and reminder flows and verify them against a disposable SQLite database.

## Technical Context

- C# and .NET 10 with ASP.NET Core MVC, REST, Swagger, EF Core and SQLite.
- Local browser and two local processes with one seeded demo user. Tailwind loads from a CDN.
- Build the solution and run a PowerShell smoke test against HTTP and rendered forms. The demo has no throughput target.

## Constitution Check

Pass before research and after design. Keep the existing architecture and schema, validate writes and isolate smoke data. Verification exercises both HTTP entry points. Existing records and routes retain their format.

## Project Structure

- `PetCareHub.WebApp` owns MVC actions, API client and views.
- `PetCareHub.Api` owns REST routes and database configuration.
- `PetCareHub.Domain` owns validation and reminder strategies.
- `PetCareHub.Database` owns data access and the SQLite dependency. `PetCareHub.Shared` owns results.
- This feature folder holds the specification, research, data model, contract and quickstart. `scripts/smoke-mvp.ps1` checks the running apps.

## Implementation

1. Preserve the existing upcoming window and document it in the UI. Validate reminder due dates and preserve the Checkup category.
2. Handle expected missing pets and rejected completions in MVC. Protect write forms and expose form validation and mobile navigation.
3. Patch the SQLite bundle, permit a separate database for verification, and align the README with the target framework.
4. Build, run the smoke scenarios and restart the API against the same test database to confirm persistence.


## Reminder management follow-up

Add a pending list query alongside the existing upcoming query. Use the same reminder display mapping and owner filter. Point the Reminders page at the pending query and leave the Dashboard on the upcoming query.

Add reminder lookup and edit operations for the existing owner. Update the editable fields only while the record is still pending. Keep existing SQLite tables and columns. Use the existing MVC form validation and antiforgery conventions. The [HTTP contract](contracts/http.md) describes the API routes, and the [task list](tasks.md) tracks implementation and verification.
