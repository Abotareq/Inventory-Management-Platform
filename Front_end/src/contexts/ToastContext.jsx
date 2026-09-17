import { createContext, useCallback, useMemo, useRef, useState } from 'react';

export const ToastContext = createContext(null);

const DEFAULT_DURATION = 4500;
const ERROR_DURATION = 7000;

export function ToastProvider({ children }) {
  const [toasts, setToasts] = useState([]);
  const counter = useRef(0);

  const removeToast = useCallback((id) => {
    setToasts((current) => current.filter((t) => t.id !== id));
  }, []);

  const addToast = useCallback(
    ({ type = 'info', message, title, duration }) => {
      const id = ++counter.current;
      const ttl = duration ?? (type === 'error' ? ERROR_DURATION : DEFAULT_DURATION);
      setToasts((current) => [...current, { id, type, message, title }]);
      if (ttl > 0) {
        window.setTimeout(() => removeToast(id), ttl);
      }
      return id;
    },
    [removeToast],
  );

  const value = useMemo(
    () => ({ toasts, addToast, removeToast }),
    [toasts, addToast, removeToast],
  );

  return <ToastContext.Provider value={value}>{children}</ToastContext.Provider>;
}
