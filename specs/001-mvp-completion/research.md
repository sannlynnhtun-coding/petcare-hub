# Phase 0 Research

## MVP scope

**Decision** — Use the README and existing routes as the acceptance baseline. **Rationale** — They define a single-user learning demo with pet profiles and care tasks. **Alternative considered** — Exposing the seeded health logs would add a separate workflow.

## Upcoming reminders and categories

**Decision** — Keep the existing UTC 14-day window and overdue behavior, describe it in the UI and add a Checkup strategy. **Rationale** — Both dashboard and reminder list depend on the same query, and Checkup already appears in the form and seed data. **Alternative considered** — Showing all pending reminders would change the endpoint contract.

## SQLite and verification

**Decision** — Update the existing SQLite bundle and support a connection string override. **Rationale** — The build flags the native dependency and a disposable database keeps smoke writes separate from the demo file. **Alternative considered** — A larger EF upgrade or replacing the database provider adds changes outside the flows. [SQLite advisory](https://github.com/advisories/GHSA-2m69-gcr7-jv3q) · [Bundle package](https://www.nuget.org/packages/SQLitePCLRaw.bundle_e_sqlite3/2.1.14)

## MVC requests

**Decision** — Use the MVC antiforgery filter and form tag helpers, render validation summaries and distinguish expected API failures from success. **Rationale** — These extend the existing forms and result envelope. **Alternative considered** — A separate client-side submission layer would duplicate MVC behavior.
