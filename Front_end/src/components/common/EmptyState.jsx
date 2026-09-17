import styles from './EmptyState.module.css';

// Empty states are invitations: say what's missing and offer the next step.
export default function EmptyState({ title, description, action, compact = false }) {
  return (
    <div className={[styles.empty, compact && styles.compact].filter(Boolean).join(' ')}>
      <p className={styles.title}>{title}</p>
      {description && <p className={styles.description}>{description}</p>}
      {action && <div className={styles.action}>{action}</div>}
    </div>
  );
}
