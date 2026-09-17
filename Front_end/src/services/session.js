import { STORAGE_KEYS } from '../config/constants';

// Thin wrapper over localStorage so token handling lives in one place.
// Every read is guarded: storage can be unavailable in private windows.

function read(key) {
  try {
    return window.localStorage.getItem(key);
  } catch {
    return null;
  }
}

function write(key, value) {
  try {
    if (value === null || value === undefined) {
      window.localStorage.removeItem(key);
    } else {
      window.localStorage.setItem(key, value);
    }
  } catch {
    // Storage unavailable; the session simply won't persist across reloads.
  }
}

export function getAccessToken() {
  return read(STORAGE_KEYS.ACCESS_TOKEN);
}

export function getRefreshToken() {
  return read(STORAGE_KEYS.REFRESH_TOKEN);
}

export function getStoredUser() {
  const raw = read(STORAGE_KEYS.USER);
  if (!raw) return null;
  try {
    return JSON.parse(raw);
  } catch {
    return null;
  }
}

export function saveSession({ accessToken, refreshToken, user }) {
  write(STORAGE_KEYS.ACCESS_TOKEN, accessToken);
  write(STORAGE_KEYS.REFRESH_TOKEN, refreshToken);
  write(STORAGE_KEYS.USER, user ? JSON.stringify(user) : null);
}

export function saveTokens({ accessToken, refreshToken }) {
  write(STORAGE_KEYS.ACCESS_TOKEN, accessToken);
  if (refreshToken) write(STORAGE_KEYS.REFRESH_TOKEN, refreshToken);
}

export function clearSession() {
  write(STORAGE_KEYS.ACCESS_TOKEN, null);
  write(STORAGE_KEYS.REFRESH_TOKEN, null);
  write(STORAGE_KEYS.USER, null);
}

// Fired on window whenever the session is forcibly ended (refresh failed).
// AuthContext listens so React state stays in sync with storage.
export const SESSION_EXPIRED_EVENT = 'session:expired';

export function announceSessionExpired() {
  window.dispatchEvent(new Event(SESSION_EXPIRED_EVENT));
}
