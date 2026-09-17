import { Link } from 'react-router-dom';
import Badge from '../common/Badge';
import EmptyState from '../common/EmptyState';
import Table from '../common/Table';
import AuditChanges from './AuditChanges';
import { formatDateTime } from '../../utils/formatDate';
import { shortId } from '../../utils/formatId';
import styles from './AuditLogList.module.css';

const ACTION_TONE = { Created: 'success', Modified: 'info', Deleted: 'danger' };

// Where an entity can be opened in the UI. Stock detail needs router state, so it links to the browser.
function entityLink(entityName, entityId) {
  switch (entityName) {
    case 'Order':
      return `/orders/${entityId}`;
    case 'Product':
      return `/stock?productId=${entityId}`;
    case 'Warehouse':
      return `/stock?warehouseId=${entityId}`;
    default:
      return null;
  }
}

export default function AuditLogList({ logs, loading, error, onRetry, filtered, onFilterEntity }) {
  const columns = [
    {
      key: 'timestamp',
      header: 'When',
      mono: true,
      width: '1%',
      render: (r) => <span className={styles.nowrap}>{formatDateTime(r.timestamp)}</span>,
    },
    {
      key: 'action',
      header: 'Action',
      render: (r) => <Badge tone={ACTION_TONE[r.action] ?? 'neutral'}>{r.action}</Badge>,
    },
    {
      key: 'entityName',
      header: 'Entity',
      render: (r) => {
        const to = entityLink(r.entityName, r.entityId);
        return (
          <div className={styles.entity}>
            <span>{r.entityName}</span>
            <span className={[styles.entityId, 'num'].join(' ')}>
              {to ? (
                <Link to={to} title={r.entityId}>{shortId(r.entityId)}</Link>
              ) : (
                <span title={r.entityId}>{shortId(r.entityId)}</span>
              )}
              <button
                type="button"
                className={styles.filterBtn}
                onClick={() => onFilterEntity(r.entityName, r.entityId)}
                aria-label={`Show only changes to this ${r.entityName}`}
                title="Show only this record"
              >
                filter
              </button>
            </span>
          </div>
        );
      },
    },
    { key: 'changes', header: 'Changes', render: (r) => <AuditChanges changes={r.changes} entityName={r.entityName} /> },
    {
      key: 'performedByUserId',
      header: 'By',
      mono: true,
      hideBelow: 'md',
      render: (r) => (
        <span className={styles.dim} title={r.performedByUserId}>
          {shortId(r.performedByUserId)}
        </span>
      ),
    },
  ];

  return (
    <Table
      caption="Audit log"
      columns={columns}
      rows={logs ?? []}
      rowKey="id"
      loading={loading}
      error={error}
      onRetry={onRetry}
      dense
      emptyState={
        <EmptyState
          compact
          title={filtered ? 'No entries match' : 'No audit entries yet'}
          description={
            filtered
              ? 'Nothing matches these filters. Clear them to see the whole log.'
              : 'Every create, update and delete is recorded here as it happens.'
          }
        />
      }
    />
  );
}
