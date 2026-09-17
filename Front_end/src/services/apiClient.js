import axios from 'axios';
import { API_BASE_URL } from '../config/config';
import {
  announceSessionExpired,
  clearSession,
  getAccessToken,
  getRefreshToken,
  saveTokens,
} from './session';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
  timeout: 30_000,
});

// ---------------------------------------------------------------------------
// Request: attach the bearer token to every call.
// ---------------------------------------------------------------------------
apiClient.interceptors.request.use((config) => {
  const token = getAccessToken();
  if (token) {
    config.headers = config.headers ?? {};
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// ---------------------------------------------------------------------------
// Response: on 401 (outside /auth/*), refresh once and retry the request.
// Concurrent 401s share one in-flight refresh promise.
// ---------------------------------------------------------------------------
let refreshPromise = null;

function isAuthRoute(url = '') {
  return /(^|\/)auth\//.test(url);
}

async function refreshTokens() {
  const refreshToken = getRefreshToken();
  const accessToken = getAccessToken();
  if (!refreshToken) {
    throw new Error('No refresh token available');
  }
  // Use a bare axios call so this request bypasses the interceptors above.
  const { data } = await axios.post(
    `${API_BASE_URL}/auth/refresh`,
    { accessToken, refreshToken },
    { headers: { 'Content-Type': 'application/json' }, timeout: 15_000 },
  );
  if (!data?.accessToken) {
    throw new Error('Refresh response did not include an access token');
  }
  saveTokens({ accessToken: data.accessToken, refreshToken: data.refreshToken });
  return data.accessToken;
}

function endSession() {
  clearSession();
  announceSessionExpired();
}

apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const { config, response } = error;

    const shouldRefresh =
      response?.status === 401 &&
      config &&
      !config._retried &&
      !isAuthRoute(config.url) &&
      getRefreshToken();

    if (!shouldRefresh) {
      return Promise.reject(error);
    }

    config._retried = true;

    try {
      if (!refreshPromise) {
        refreshPromise = refreshTokens().finally(() => {
          refreshPromise = null;
        });
      }
      const newToken = await refreshPromise;
      config.headers = config.headers ?? {};
      config.headers.Authorization = `Bearer ${newToken}`;
      return apiClient(config);
    } catch {
      endSession();
      return Promise.reject(error);
    }
  },
);

// ---------------------------------------------------------------------------
// Error helpers. The API returns RFC 9110 ProblemDetails: `title` carries the
// human-readable message; validation failures add `errors` keyed by field.
// ---------------------------------------------------------------------------

const STATUS_FALLBACKS = {
  400: 'The request was not valid. Check the form and try again.',
  401: 'Your session has ended. Sign in again to continue.',
  403: "You don't have permission to do that.",
  404: "That record doesn't exist or was removed.",
  409: 'That change conflicts with the current state. Refresh and try again.',
  500: 'Something went wrong on the server. Try again in a moment.',
};

function flattenValidationErrors(errors) {
  if (!errors || typeof errors !== 'object') return [];
  return Object.values(errors)
    .flatMap((v) => (Array.isArray(v) ? v : [v]))
    .filter((m) => typeof m === 'string' && m.trim().length > 0);
}

export function extractErrorMessage(error) {
  if (!error) return 'Something went wrong.';

  if (axios.isCancel(error) || error.code === 'ERR_CANCELED') {
    return 'The request was cancelled.';
  }

  if (!error.response) {
    if (error.code === 'ECONNABORTED') {
      return 'The server took too long to respond. Try again.';
    }
    return "Can't reach the server. Check your connection and try again.";
  }

  const { status, data } = error.response;

  if (data && typeof data === 'object') {
    const validation = flattenValidationErrors(data.errors);
    if (validation.length > 0) {
      return validation.join(' ');
    }
    if (typeof data.title === 'string' && data.title.trim()) {
      return data.title;
    }
    if (typeof data.detail === 'string' && data.detail.trim()) {
      return data.detail;
    }
  }

  if (typeof data === 'string' && data.trim()) {
    return data;
  }

  return STATUS_FALLBACKS[status] ?? `Request failed (${status}).`;
}

// Returns { fieldName: 'message' } for validation responses so forms can show
// errors inline. Keys are camel-cased to match input names.
export function extractFieldErrors(error) {
  const errors = error?.response?.data?.errors;
  if (!errors || typeof errors !== 'object') return {};
  const result = {};
  for (const [key, value] of Object.entries(errors)) {
    const field = key.charAt(0).toLowerCase() + key.slice(1);
    const messages = Array.isArray(value) ? value : [value];
    result[field] = messages.filter(Boolean).join(' ');
  }
  return result;
}

export function isConflictError(error) {
  return error?.response?.status === 409;
}

export default apiClient;
