import { useNavigate } from 'react-router-dom';
import Button from '../common/Button';
import EmptyState from '../common/EmptyState';
import Table from '../common/Table';
import OrderStatusBadge from '../orders/OrderStatusBadge';
import { formatCurrency } from '../../utils/formatCurrency';
import { formatRelative } from '../../utils/formatDate';
import { shortId } from '../../utils/formatId';

export default function RecentOrders({ orders, loading, error, onRetry, canCreate }) {
  const navigate = useNavigate();
  const columns = [
    { key: 'orderId', header: 'Order', mono: true, render: (r) => shortId(r.orderId) },
    { key: 'status', header: 'Status', render: (r) => <OrderStatusBadge status={r.status} /> },
    {
      key: 'totalAmount',
      header: 'Total',
      align: 'right',
      mono: true,
      render: (r) => formatCurrency(r.totalAmount),
    },
    {
      key: 'createdAt',
      header: 'Created',
      hideBelow: 'sm',
      render: (r) => <span title={r.createdAt}>{formatRelative(r.createdAt)}</span>,
    },
  ];

  return (
    <Table
      caption="Recent orders"
      columns={columns}
      rows={orders ?? []}
      rowKey="orderId"
      loading={loading}
      error={error}
      onRetry={onRetry}
      onRowClick={(r) => navigate(`/orders/${r.orderId}`)}
      dense
      emptyState={
        <EmptyState
          compact
          title="No orders yet"
          description={canCreate ? 'Create your first order to get started.' : 'Orders will show up here as sales agents create them.'}
          action={canCreate ? <Button variant="primary" size="sm" to="/orders/new">Create order</Button> : null}
        />
      }
    />
  );
}
