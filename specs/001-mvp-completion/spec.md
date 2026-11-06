# PetCare Hub MVP completion

Scope is derived from the repository README. A local demo owner can view dashboard totals, list pets, add a pet and view its details. The owner can create a reminder for an existing pet, see upcoming care tasks and mark a reminder done.

- Creation persists across API restarts. Upcoming includes unfinished overdue reminders and those through UTC today plus 14 days, ordered by due date.
- Missing or invalid form input produces visible feedback. A failed completion shows an error and a missing pet returns to the list.
- All primary navigation is available on narrow screens. Browser write forms require an antiforgery token.
- The existing API-only pet update remains supported. Health log management, authentication and real notifications are outside this demo.


## Reminder management follow-up

- FR-001. Show all unfinished reminders on the Reminders page, including overdue items and reminders due more than 14 days away. Sort by due date. Keep the Dashboard on its existing upcoming window.
- FR-002. Open a pending reminder from the Reminders page and prefill its title, type, due date and notes. Save valid edits and return to the pending list with the updated item visible. Cancel returns to the list without saving. The linked pet stays fixed.
- FR-003. Show validation feedback and retain entered values for invalid edits. Reject edits to missing, unowned or completed reminders. A completion that wins a concurrent race must prevent a stale edit from updating the completed reminder.
- SC-001. A reminder due 30 days from today appears immediately in the pending list and stays outside the Dashboard window. Moving its due date to tomorrow makes it appear in the Dashboard upcoming results. Its updated fields persist after an API restart.

Completed history, Pet Edit and Health Logs are separate scope decisions.
