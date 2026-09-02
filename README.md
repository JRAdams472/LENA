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
├── LENA.Database/            # SQL Server project: schemas, tables, indexes, seed data, stored procedures
├── mobile/                   # Flutter mobile app
└── README.md                 # This file
```

---

## Quick Start

### 1. Database

All DDL lives in `LENA.Database/`, split by domain (`Wine/`, `Inventory/`, `MealPlan/`, `Recipe/`). Each domain has `Schema.sql`, `Tables/`, `Indexes/` and `StoredProcedures/`; seed data lives in `Wine/Seed/` and `SeedData/`.

With Docker, nothing to do: `docker compose up --build` provisions the database automatically (see [Docker](#docker)).

For a non-Docker setup (LocalDB or an existing SQL Server), create the `LENA` database and apply the fragments in dependency order — schemas, tables, indexes, seed data, then stored procedures:

```bash
sqlcmd -S "(localdb)\mssqllocaldb" -Q "IF DB_ID(N'LENA') IS NULL CREATE DATABASE [LENA]"
for f in LENA.Database/*/Schema.sql \
         LENA.Database/{Wine,Inventory,MealPlan,Recipe}/Tables/*.sql \
         LENA.Database/*/Indexes/*.sql \
         LENA.Database/Wine/Seed/*.sql LENA.Database/SeedData/*.sql \
         LENA.Database/*/StoredProcedures/*.sql; do
  sqlcmd -S "(localdb)\mssqllocaldb" -d LENA -b -i "$f"
done
```

Tables carry inline foreign keys, so a file may need to be retried after the table it references exists; `LENA.Database/init.sh` (used by Docker) handles that ordering for you.

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

The API enforces **fallback authorization**: every controller action requires an authenticated user by default. Endpoints can opt out explicitly with `[AllowAnonymous]` if public access is ever required. The browser origin is also restricted to the configured `Cors:AllowedOrigins` list.

### 2. Run the API

From the repo root:

```bash
dotnet run --project LENA.API
```

By default the API listens on `http://localhost:5059` as configured in `LENA.API/Properties/launchSettings.json`. The HTTPS profile also exposes `https://localhost:7284`.

The API includes Swagger UI in development at `http://localhost:5059/swagger`. Outside development it is off unless `Swagger:Enabled` is `true`; `Swagger:RoutePrefix` moves it behind a reverse proxy subpath (the Docker stack sets both, serving it at `http://localhost/api/swagger`).

### Configure Google Sign-In

Login requires a Google OAuth Web client ID that is shared by the API, frontend, and mobile app. See [docs/google-oauth-client-id.md](docs/google-oauth-client-id.md) for how to create the client ID and add the authorized origins.

Use the same client ID in these three places:

- `LENA.API/appsettings.Development.json` or user secrets: `Authentication:Google:ClientId`
- `frontend/.env.local`: `NEXT_PUBLIC_GOOGLE_CLIENT_ID`
- Mobile: `GOOGLE_SERVER_CLIENT_ID` dart-define

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

The API registers an `AllowExternal` CORS policy that only allows the origins listed in `Cors:AllowedOrigins` (e.g. `http://localhost:3000` in development). If you serve the frontend from a different origin, add it to `LENA.API/appsettings.json` or `LENA.API/appsettings.Development.json`:

```json
"Cors": {
  "AllowedOrigins": [ "http://localhost:3000" ]
}
```

The configured origins are read at startup and the app will fail to start if the list is empty.

---

## Docker

Run the entire stack from the repo root:

```bash
docker compose up --build
```

The one-shot `db-init` service runs `LENA.Database/init.sh` against the `db` container: it waits for SQL Server, creates the `LENA` database if missing, and applies every `.sql` fragment (schemas → tables → indexes → seed data → stored procedures). The `api` service only starts once `db-init` has completed successfully.

Init is idempotent — existing schemas, tables, indexes and already-populated seed tables are skipped, and stored procedures are applied as `CREATE OR ALTER` — so re-running `docker compose up` against an existing `mssql_data` volume is safe. Use `docker compose down -v` to start from an empty database.

Once the containers are healthy, the whole application is available on a single origin:

- **Web app**: http://localhost
- **Swagger UI**: http://localhost/api/swagger

### Routing

Caddy (the `proxy` service) reverse-proxies all traffic:

- `/api/*` → `api` service (ASP.NET Core API on port 8080)
- All other paths → `ui` service (Next.js on port 3000)

### Going live

To run on a real domain with automatic HTTPS:

1. Open `Caddyfile` and replace `http://localhost` with your domain (e.g. `lena.example.com`).
2. In `docker-compose.yml`, update `Cors__AllowedOrigins__0` to the same public origin (e.g. `https://lena.example.com`).
3. Ensure the host is publicly reachable on ports 80/443 and DNS points the domain to it. Caddy provisions and renews the Let's Encrypt certificate automatically.

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

- See `LENA.Database/` for the complete database design
- See `frontend/README.md` for frontend-specific details
- Development notes in `notes.md`

---

## License

Personal use only.
