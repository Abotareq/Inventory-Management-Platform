// API base URL. Defaults to the Vite dev proxy (see vite.config.js), which
// forwards /api to the local ASP.NET Core dev server without needing CORS.
// Override with VITE_API_BASE_URL for staging/production builds.
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || '/api';

export const APP_NAME = 'Inventory';
