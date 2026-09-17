import EmptyState from '../common/EmptyState';
import Table from '../common/Table';
import { formatDateTime } from '../../utils/formatDate';
import { shortId } from '../../utils/formatId';
import styles from './StockHistory.module.css';

// Adjustment history for one stock record (signed deltas with resulting quantity).
export default function StockHistory({ rows, loading, error, onRetry }) {
  const columns = [
    {
      key: 'timestamp',
      header: 'When',
      mono: true,
      width: '1%',
      render: (row) => <span className={styles.nowrap}>{formatDateTime(row.timestamp)}</span>,
    },
    {
      key: 'delta',
      header: 'Change',
      align: 'right',
      mono: true,
      render: (row) => (
        <span className={row.delta < 0 ? styles.negative : styles.positive}>
          {row.delta > 0 ? `+${row.delta}` : row.delta}
        </span>
      ),
    },
    { key: 'resultingQuantity', header: 'Resulting', align: 'right', mono: true },
    { key: 'reason', header: 'Reason' },
    {
      key: 'performedByUserId',
      header: 'By',
      mono: true,
      hideBelow: 'md',
      render: (row) => (
        <span className={styles.dim} title={row.performedByUserId}>
          {shortId(row.performedByUserId)}
        </span>
      ),
    },
  ];

  return (
    <Table
      caption="Adjustment history"
      columns={columns}
      rows={rows ?? []}
      rowKey="stockAdjustmentId"
      loading={loading}
      error={error}
      onRetry={onRetry}
      dense
      emptyState={
        <EmptyState
          compact
          title="No adjustments yet"
          description="Every stock change made by a warehouse operator will be listed here with its reason."
        />
      }
    />
  );
}
