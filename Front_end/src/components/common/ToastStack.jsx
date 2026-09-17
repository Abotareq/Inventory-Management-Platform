import { useToast } from '../../hooks/useToast';
import styles from './ToastStack.module.css';

const TONE_LABEL = { success: 'Done', error: 'Error', info: 'Notice' };

export default function ToastStack() {
  const { toasts, remove } = useToast();
  if (toasts.length === 0) return null;

  return (
    <div className={styles.stack} aria-live="polite" aria-relevant="additions">
      {toasts.map((t) => (
        <div
          key={t.id}
          className={[styles.toast, styles[t.type]].join(' ')}
          role={t.type === 'error' ? 'alert' : 'status'}
        >
          <div className={styles.body}>
            <span className={styles.kind}>{t.title ?? TONE_LABEL[t.type] ?? 'Notice'}</span>
            <span className={styles.message}>{t.message}</span>
          </div>
          <button
            type="button"
            className={styles.dismiss}
            onClick={() => remove(t.id)}
            aria-label="Dismiss notification"
          >
            <span aria-hidden="true">×</span>
          </button>
        </div>
      ))}
    </div>
  );
}
