# Booking System API

A RESTful API for a booking management system built with ASP.NET Core and MS SQL Server, following the Vertical Slice Architecture.

> **Note:** This project is currently under active development. Features and structure are subject to change.

## Tech Stack

* **Framework:** ASP.NET Core
* **Database:** MS SQL Server (Entity Framework Core)
* **Containerization:** Docker & Docker Compose
* **Architecture:** Vertical Slice Architecture (Feature-driven approach)

---

## Project Structure

The project is organized by vertical slices (`Features`), making each business capability isolated and easy to maintain:

```text
BookingSystem.Api/
│
├── Domain/                 # Domain models (Booking, Slot)
├── Features/               # Business capabilities (Handlers, Endpoints)
│   ├── Bookings/           # Booking features (Create, Cancel)
│   └── Slots/              # Slot features (Create, GetAvailable)
├── Persistence/            # Database context, migrations, and EF configurations
├── Properties/             # Launch settings
├── Dockerfile              # Docker build configuration for the API
└── BookingSystem.Api.csproj

```

---

## Endpoints / Features

* **Slots (`/api/slots`):**
* `POST` — Create a new slot (`CreateSlot`)


* `GET` — Retrieve available slots (`GetAvailableSlots`)




* **Bookings (`/api/bookings`):**
* `POST` — Create a booking (`CreateBooking`)


* `POST / {id} / cancel` — Cancel a booking (`CancelBooking`)