import { useContext, useMemo } from 'react';
import { ToastContext } from '../contexts/ToastContext';

export function useToast() {
  const ctx = useContext(ToastContext);
  if (!ctx) {
    throw new Error('useToast must be used inside <ToastProvider>');
  }
  const { addToast, removeToast, toasts } = ctx;

  return useMemo(
    () => ({
      toasts,
      remove: removeToast,
      success: (message, opts) => addToast({ type: 'success', message, ...opts }),
      error: (message, opts) => addToast({ type: 'error', message, ...opts }),
      info: (message, opts) => addToast({ type: 'info', message, ...opts }),
    }),
    [addToast, removeToast, toasts],
  );
}
