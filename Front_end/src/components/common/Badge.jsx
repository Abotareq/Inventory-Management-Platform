import styles from './Badge.module.css';

// tone: accent | success | danger | info | neutral
// Status is always conveyed by colour and text together; the text is the label.
export default function Badge({ tone = 'neutral', children, className, title }) {
  return (
    <span
      className={[styles.badge, styles[tone] ?? styles.neutral, className]
        .filter(Boolean)
        .join(' ')}
      title={title}
    >
      <span className={styles.dot} aria-hidden="true" />
      {children}
    </span>
  );
}
