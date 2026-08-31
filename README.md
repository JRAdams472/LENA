# LENA - The Smart Kitchen Assistant

A personal kitchen management system for tracking food inventory, recipes, meal planning, and wine collection.

## Overview

LENA helps you manage your kitchen by keeping track of what you have, what you want to cook, and pairing meals with your wine collection.

### Features

- **Food Inventory**: Track pantry items, expiration dates, and quantities
- **Recipe Management**: Store and organize your favorite recipes
- **Meal Planning**: Plan weekly meals with smart ingredient optimization
- **Grocery List**: Automatically generate shopping lists from meal plans
- **Wine Collection**: Track your wine inventory with purchase history

### Technology

- **Database**: MS SQL Server
- **Backend**: ASP.NET Core Web API
- **Frontend**: Next.js 16 (App Router) + TypeScript + Material UI
- **Platform**: Local/Server-based
- **Privacy First**: No cloud dependencies

---

## Project Structure

```
LENA/
├── LENA.API/                 # ASP.NET Core API
├── LENA.Application/         # MediatR handlers, repositories, validators
├── LENA/                     # Domain entities
├── LENA.Application.UnitTests/
├── LENA.API.UnitTests/
├── frontend/                 # Next.js frontend
│   ├── app/                  # App Router pages
│   ├── lib/                  # Typed API client and types
│   └── README.md             # Frontend-specific instructions
├── SQL/                      # Database schema, seed data, stored procedures
└── README.md                 # This file
```

---

## Quick Start

### 1. Database

Create the database using `SQL/schema.sql`, then optionally load sample data from `SQL/seed.sql`.

The API expects the `DefaultConnection` string in `LENA.API/appsettings.Development.json` (or user secrets / environment variables outside development):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LENA;Trusted_Connection=True;"
}
```

Update this to point to your SQL Server or LocalDB instance. The API fails to start when it is missing.

### Configuration and security

The API also requires the browser origins that may call it:

```json
"Cors": {
  "AllowedOrigins": [ "http://localhost:3000" ]
}
```

Startup fails when `Cors:AllowedOrigins` is empty, so a deployment cannot silently fall back to allowing any origin.

The API currently has **no authentication or authorization**: every endpoint is open to anyone who can reach it, and CORS only limits which browser origins may call it. Do not expose it beyond a trusted local network until auth is added. Audit fields (`CreatedBy` / `LastUpdatedBy`) are stamped server-side from the request identity and fall back to `system` while the API is unauthenticated.

### 2. Run the API

From the repo root:

```bash
dotnet run --project LENA.API
```

By default the API listens on `http://localhost:5059` as configured in `LENA.API/Properties/launchSettings.json`. The HTTPS profile also exposes `https://localhost:7284`.

The API includes Swagger UI in development at `http://localhost:5059/swagger`.

### 3. Run the Frontend

From the `frontend/` directory:

```bash
cd frontend
npm install
```

Copy the environment example and confirm the API origin:

```bash
cp .env.example .env.local
```

Edit `.env.local` to match the running API port:

```
NEXT_PUBLIC_API_BASE_URL=http://localhost:5059
NEXT_PUBLIC_API_URL=http://localhost:5059
```

Start the dev server:

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000).

### 4. Confirm CORS

The API registers an `AllowExternal` CORS policy allowing any origin, header, and method. If you serve the frontend from a different origin, verify that the API's CORS configuration in `LENA.API/Program.cs` covers it:

```csharp
options.AddPolicy("AllowExternal", policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
});
```

---

## Build for Production

### API

```bash
dotnet build
```

### Frontend

```bash
cd frontend
npm run build
```

---

## Getting Help

- See `SQL/schema.sql` for the complete database design
- See `frontend/README.md` for frontend-specific details
- Development notes in `notes.md`

---

## License

Personal use only.
