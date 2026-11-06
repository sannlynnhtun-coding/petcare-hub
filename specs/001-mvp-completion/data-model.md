# Data Model

| Entity | Fields and rules |
| --- | --- |
| User | ID, email, password hash, display name and creation time. Demo owner ID is fixed. |
| Pet | ID, owner ID, name, species, optional breed, birth date, gender, notes and photo URL, creation time and soft-delete flag. Name and species must contain non-whitespace text. |
| Reminder | ID, pet ID, type, title, optional notes, due date, creation time, completion flag and optional completion time. The pet must belong to the demo owner. Title and due date are required. |
| HealthLog | ID, pet ID, type, title, optional notes and value, date and creation time. Seeded storage only. |

A user owns many pets. Each pet has many reminders and health logs. A pending reminder can become completed once and receives a UTC completion timestamp. Completed reminders leave the upcoming query. Existing records use the same schema.
