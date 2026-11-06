# HTTP and UI Contract

| Route | Behavior |
| --- | --- |
| GET /api/pets | Lists the demo owner's visible pets. |
| GET /api/pets/{id} | Returns the pet or 404. |
| POST /api/pets | Creates a pet from name, species and optional profile fields. |
| PUT /api/pets/{id} | Updates an owned pet with the same profile fields. |
| GET /api/reminders/upcoming | Lists incomplete reminders through UTC today plus 14 days, including overdue items. |
| GET /api/reminders/pending | Lists all incomplete reminders for the demo owner, sorted by due date. |
| GET /api/reminders/{id} | Returns an owned pending reminder's editable fields with the stored title, or 404. |
| PUT /api/reminders/{id} | Updates the title, type, due date and notes on an owned pending reminder. Invalid or unavailable reminders return 400. |
| POST /api/reminders | Creates a reminder from petId, type, title, dueDate and optional notes. |
| POST /api/reminders/{id}/complete | Completes an owned pending reminder. Missing and already completed IDs return 400. |

Successful actions return HTTP 200 with `isSuccess`, `message` and `data` where applicable. Domain validation failures use HTTP 400 and the result envelope. Framework binding failures may return validation problem details.

The dashboard is at `/`, with pet and reminder pages under `/Pets` and `/Reminders`. MVC writes use antiforgery tokens. Create forms display validation errors and successful writes redirect. A failed completion redirects with an error banner. The Reminders page lists all pending reminders and provides an Edit link for each item. The edit form shows validation errors and keeps the entered fields on rejection.
