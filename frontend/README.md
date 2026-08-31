# LENA Frontend

A Next.js 16 (App Router) TypeScript frontend for the LENA admin dashboard.

## Requirements

- Node.js 20+
- A running `LENA.API` instance (default: `http://localhost:5059`)

## Getting Started

1. Install dependencies:

   ```bash
   npm install
   ```

2. Configure the API base URL. Copy `.env.example` to `.env.local` and update the origin to match the API:

   ```bash
   cp .env.example .env.local
   ```

   Example `.env.local`:

   ```
   NEXT_PUBLIC_API_BASE_URL=http://localhost:5059
   NEXT_PUBLIC_API_URL=http://localhost:5059
   ```

3. Start the development server:

   ```bash
   npm run dev
   ```

   Open [http://localhost:3000](http://localhost:3000) in your browser.

4. Confirm CORS origin matches the API. The API `Program.cs` is configured with `AllowExternal` allowing any origin, header, and method. If you change the frontend origin, ensure the API CORS policy allows it.

## Build

```bash
npm run build
```

## Tech Stack

- Next.js (App Router)
- TypeScript
- Material UI
- React Query
