import { createContext, useCallback, useEffect, useMemo, useState } from 'react';
import * as authService from '../services/authService';
import {
  SESSION_EXPIRED_EVENT,
  clearSession,
  getAccessToken,
  getStoredUser,
  saveSession,
} from '../services/session';

export const AuthContext = createContext(null);

function readInitialUser() {
  const user = getStoredUser();
  const token = getAccessToken();
  return user && token ? user : null;
}

export function AuthProvider({ children }) {
  const [user, setUser] = useState(readInitialUser);
  // Set when the session was ended by a failed refresh, so the login page can
  // explain why the user landed there.
  const [sessionExpired, setSessionExpired] = useState(false);

  useEffect(() => {
    function onExpired() {
      setUser(null);
      setSessionExpired(true);
    }
    window.addEventListener(SESSION_EXPIRED_EVENT, onExpired);
    return () => window.removeEventListener(SESSION_EXPIRED_EVENT, onExpired);
  }, []);

  const login = useCallback(async ({ email, password }) => {
    const data = await authService.login({ email, password });
    const nextUser = {
      userId: data.userId,
      fullName: data.fullName,
      email: data.email,
      role: data.role,
    };
    saveSession({
      accessToken: data.accessToken,
      refreshToken: data.refreshToken,
      user: nextUser,
    });
    setSessionExpired(false);
    setUser(nextUser);
    return nextUser;
  }, []);

  const logout = useCallback(() => {
    clearSession();
    setSessionExpired(false);
    setUser(null);
  }, []);

  const value = useMemo(
    () => ({
      user,
      isAuthenticated: Boolean(user),
      sessionExpired,
      login,
      logout,
    }),
    [user, sessionExpired, login, logout],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
