import Button from './Button';
import styles from './ErrorMessage.module.css';

// Inline error panel. `onRetry` adds a retry action.
export default function ErrorMessage({ title = 'Something went wrong', message, onRetry, className }) {
  return (
    <div className={[styles.panel, className].filter(Boolean).join(' ')} role="alert">
      <div className={styles.body}>
        <p className={styles.title}>{title}</p>
        {message && <p className={styles.message}>{message}</p>}
      </div>
      {onRetry && (
        <Button size="sm" onClick={onRetry}>
          Try again
        </Button>
      )}
    </div>
  );
}
