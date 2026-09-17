import { Link } from 'react-router-dom';
import Badge from '../common/Badge';
import EmptyState from '../common/EmptyState';
import Table from '../common/Table';
import { RESERVATION_ACTION_TONE } from '../../config/constants';
import { formatDateTime } from '../../utils/formatDate';
import { shortId } from '../../utils/formatId';
import styles from './StockHistory.module.css';

// Reservation history for one stock record: Reserved / Released / Committed per order.
export default function StockReservationHistory({ rows, loading, error, onRetry }) {
  const columns = [
    {
      key: 'timestamp',
      header: 'When',
      mono: true,
      width: '1%',
      render: (row) => <span className={styles.nowrap}>{formatDateTime(row.timestamp)}</span>,
    },
    {
      key: 'action',
      header: 'Action',
      render: (row) => (
        <Badge tone={RESERVATION_ACTION_TONE[row.action] ?? 'neutral'}>{row.action}</Badge>
      ),
    },
    { key: 'amount', header: 'Amount', align: 'right', mono: true },
    {
      key: 'orderId',
      header: 'Order',
      mono: true,
      render: (row) => (
        <Link to={`/orders/${row.orderId}`} title={row.orderId}>
          {shortId(row.orderId)}
        </Link>
      ),
    },
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
      caption="Reservation history"
      columns={columns}
      rows={rows ?? []}
      rowKey="stockReservationId"
      loading={loading}
      error={error}
      onRetry={onRetry}
      dense
      emptyState={
        <EmptyState
          compact
          title="No reservations yet"
          description="Stock is reserved when an order begins processing, then committed on completion or released on cancel."
        />
      }
    />
  );
}
