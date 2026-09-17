import EmptyState from '../common/EmptyState';
import ErrorMessage from '../common/ErrorMessage';
import LoadingSpinner from '../common/LoadingSpinner';
import OrderStatusBadge from './OrderStatusBadge';
import { formatDateTime } from '../../utils/formatDate';
import { shortId } from '../../utils/formatId';
import styles from './OrderHistoryTimeline.module.css';

// Status transitions in chronological order (oldest first).
export default function OrderHistoryTimeline({ entries, loading, error, onRetry }) {
  if (error) {
    return <ErrorMessage title="Couldn't load history" message={error} onRetry={onRetry} />;
  }
  if (loading && !entries) return <LoadingSpinner label="Loading history" />;

  const sorted = [...(entries ?? [])].sort(
    (a, b) => new Date(a.timestamp) - new Date(b.timestamp),
  );

  if (sorted.length === 0) {
    return (
      <EmptyState
        compact
        title="No transitions yet"
        description="Each status change will be recorded here with who made it and when."
      />
    );
  }

  return (
    <ol className={styles.timeline}>
      {sorted.map((entry) => (
        <li key={entry.orderHistoryId} className={styles.entry}>
          <span className={styles.marker} aria-hidden="true" />
          <div className={styles.body}>
            <div className={styles.transition}>
              {entry.fromStatus ? <OrderStatusBadge status={entry.fromStatus} /> : <span className={styles.dim}>Created</span>}
              <span className={styles.arrow} aria-hidden="true">→</span>
              <span className="visually-hidden">to</span>
              <OrderStatusBadge status={entry.toStatus} />
            </div>
            <div className={styles.meta}>
              <span className="num">{formatDateTime(entry.timestamp)}</span>
              <span className={styles.by}>
                by <span className="num" title={entry.performedByUserId}>{shortId(entry.performedByUserId)}</span>
              </span>
            </div>
          </div>
        </li>
      ))}
    </ol>
  );
}
