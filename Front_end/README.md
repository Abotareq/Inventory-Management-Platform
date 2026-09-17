# Inventory Management Platform — frontend

React client for the ASP.NET Core inventory/order API in this repository.

## Stack

- Vite + React (JavaScript)
- react-router-dom, axios
- CSS Modules; self-hosted fonts via `@fontsource`

## Run

```
npm install
npm run dev
```

The dev server runs on http://localhost:5173 and proxies `/api` to the backend
at `http://localhost:5116` (the `http` launch profile). Start the API first:

```
dotnet run --project ../Inventory_Management_Platform.Api --launch-profile http
```

Test accounts are listed in the root README.

## Configuration

| Variable | Purpose | Default |
|---|---|---|
| `VITE_API_BASE_URL` | Base URL the client calls | `/api` (Vite proxy) |
| `VITE_API_PROXY_TARGET` | Where the dev proxy forwards `/api` | `http://localhost:5116` |

Copy `.env.example` to `.env.local` to override. For a production build set
`VITE_API_BASE_URL` to the deployed API, and enable CORS on the backend for the
client's origin.

## Deployment

Production runs on Vercel: https://inventory-management-platform-nu.vercel.app

`vercel.json` rewrites `/api/*` to the deployed API (`http://inventoryplatform.somee.com`) so the browser only talks to the HTTPS origin (no CORS or mixed-content issues), and sends every other path to `index.html` for client-side routing. Deploy from this folder with `vercel deploy --prod`.

## Structure

```
src/
  components/   micro-components grouped by domain (common, layout, orders, …)
  pages/        one component per route
  contexts/     AuthContext, ToastContext
  hooks/        useAuth, useToast, useFetch, usePagination
  services/     apiClient (axios + interceptors) and one file per API area
  utils/        formatting and role helpers
  config/       API URL, roles, statuses
  styles/       design tokens and global CSS
```
