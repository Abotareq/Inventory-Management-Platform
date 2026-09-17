import Button from '../common/Button';
import EmptyState from '../common/EmptyState';
import Table from '../common/Table';
import OrderStatusBadge from './OrderStatusBadge';
import { formatCurrency } from '../../utils/formatCurrency';
import { formatDateTime } from '../../utils/formatDate';
import { shortId } from '../../utils/formatId';
import styles from './OrderList.module.css';

export default function OrderList({
  orders,
  loading,
  error,
  onRetry,
  filtered = false,
  canCreate,
  onCreate,
  onOpen,
}) {
  const columns = [
    {
      key: 'orderId',
      header: 'Order',
      mono: true,
      width: '1%',
      render: (row) => (
        <span className={styles.id} title={row.orderId}>
          {shortId(row.orderId)}
        </span>
      ),
    },
    {
      key: 'status',
      header: 'Status',
      render: (row) => <OrderStatusBadge status={row.status} />,
    },
    {
      key: 'customerId',
      header: 'Customer',
      mono: true,
      hideBelow: 'md',
      render: (row) => (
        <span className={styles.dim} title={row.customerId}>
          {shortId(row.customerId)}
        </span>
      ),
    },
    {
      key: 'items',
      header: 'Items',
      align: 'right',
      mono: true,
      hideBelow: 'sm',
      render: (row) => row.items?.length ?? 0,
    },
    {
      key: 'totalAmount',
      header: 'Total',
      align: 'right',
      mono: true,
      render: (row) => formatCurrency(row.totalAmount),
    },
    {
      key: 'createdAt',
      header: 'Created',
      mono: true,
      hideBelow: 'md',
      render: (row) => <span className={styles.nowrap}>{formatDateTime(row.createdAt)}</span>,
    },
  ];

  const emptyState = filtered ? (
    <EmptyState
      compact
      title="No orders match"
      description="Nothing matches these filters. Clear them to see every order."
    />
  ) : (
    <EmptyState
      compact
      title="No orders yet"
      description={
        canCreate
          ? 'Create your first order to get started.'
          : 'Orders will appear here once a sales agent creates one.'
      }
      action={
        canCreate ? (
          <Button variant="primary" onClick={onCreate}>
            Create order
          </Button>
        ) : null
      }
    />
  );

  return (
    <Table
      caption="Orders"
      columns={columns}
      rows={orders ?? []}
      rowKey="orderId"
      loading={loading}
      error={error}
      onRetry={onRetry}
      onRowClick={onOpen}
      emptyState={emptyState}
    />
  );
}
