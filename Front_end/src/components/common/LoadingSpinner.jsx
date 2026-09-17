import styles from './LoadingSpinner.module.css';

// size: sm | md
export default function LoadingSpinner({ label = 'Loading', size = 'md', inline = false }) {
  return (
    <div
      className={[styles.wrapper, inline && styles.inline].filter(Boolean).join(' ')}
      role="status"
      aria-live="polite"
    >
      <span className={[styles.spinner, styles[size]].join(' ')} aria-hidden="true" />
      <span className={inline ? 'visually-hidden' : styles.label}>{label}…</span>
    </div>
  );
}
