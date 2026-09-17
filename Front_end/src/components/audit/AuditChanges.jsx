import { ORDER_STATUSES } from '../../config/constants';
import styles from './AuditChanges.module.css';

// Order.Status is persisted as an enum ordinal in the same order as ORDER_STATUSES.
function display(entityName, field, v) {
  if (entityName === 'Order' && field === 'Status' && Number.isInteger(v) && ORDER_STATUSES[v]) {
    return ORDER_STATUSES[v];
  }
  return fmt(v);
}

// `changes` is a JSON string, usually { Field: { old, new } }. Falls back to raw text.
export default function AuditChanges({ changes, entityName }) {
  if (!changes) return <span className={styles.dim}>—</span>;

  let parsed;
  try {
    parsed = JSON.parse(changes);
  } catch {
    return <code className={styles.raw}>{changes}</code>;
  }

  if (!parsed || typeof parsed !== 'object') {
    return <code className={styles.raw}>{String(parsed)}</code>;
  }

  const entries = Object.entries(parsed);
  if (entries.length === 0) return <span className={styles.dim}>No field changes</span>;

  return (
    <dl className={styles.list}>
      {entries.map(([field, value]) => {
        const isDiff = value && typeof value === 'object' && ('old' in value || 'new' in value);
        return (
          <div key={field} className={styles.row}>
            <dt className={styles.field}>{field}</dt>
            <dd className={[styles.value, 'num'].join(' ')}>
              {isDiff ? (
                <>
                  <span className={styles.old}>{display(entityName, field, value.old)}</span>
                  <span className={styles.arrow} aria-hidden="true">→</span>
                  <span className="visually-hidden">to</span>
                  <span className={styles.new}>{display(entityName, field, value.new)}</span>
                </>
              ) : (
                display(entityName, field, value)
              )}
            </dd>
          </div>
        );
      })}
    </dl>
  );
}

function fmt(v) {
  if (v === null || v === undefined) return 'null';
  // Value objects serialise as { Value: x }; show the inner value.
  if (v && typeof v === 'object' && Object.keys(v).length === 1 && 'Value' in v) return fmt(v.Value);
  if (typeof v === 'object') return JSON.stringify(v);
  return String(v);
}
