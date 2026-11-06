# Tasks | Reminder management follow-up

- [x] T001 [FR-001] Add an owner-scoped pending query without a date cutoff in PetCareHub.Database, expose it through the domain and API, and use it on the WebApp Reminders page. Keep the Dashboard on the existing upcoming endpoint.
- [x] T002 [FR-002] Add owner-scoped lookup and update operations to the API for the edit flow. Accept title, type, due date and notes. Keep the linked pet fixed.
- [x] T003 [FR-003] Apply edit validation at the domain boundary. Condition the write on ownership and pending status, and return an error for stale, missing or unowned records.
- [x] T004 [FR-002, FR-003] Add Edit links and a prefilled MVC form with antiforgery, validation feedback, cancel navigation and a redirect on successful save. Depends on T002 and T003.
- [x] T005 [FR-001, FR-002, FR-003, SC-001] Run the scenario with a disposable database. Verify a 30-day reminder appears only in the pending list, reschedule it to tomorrow, confirm it enters Dashboard upcoming results, and restart the API to confirm persistence. Verify invalid edits preserve the stored fields and completed reminders reject edits.
