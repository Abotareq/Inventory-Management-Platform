import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';

// The backend does not configure CORS, so in development we proxy /api to the
// ASP.NET Core dev server. Set VITE_API_PROXY_TARGET to point elsewhere.
export default defineConfig(({ mode }) => {
  const target = process.env.VITE_API_PROXY_TARGET || 'http://localhost:5116';
  return {
    plugins: [react()],
    server: {
      port: 5173,
      proxy:
        mode === 'development'
          ? {
              '/api': {
                target,
                changeOrigin: true,
                secure: false,
              },
            }
          : undefined,
    },
  };
});
