import { useNavigate } from 'react-router-dom';
import EmptyState from '../common/EmptyState';
import Table from '../common/Table';
import StockLevelBadge from '../stock/StockLevelBadge';
import { LOW_STOCK_THRESHOLD } from '../../config/constants';

// rows: stock records with `product` and `warehouse` already resolved.
export default function LowStockList({ rows, loading, error, onRetry }) {
  const navigate = useNavigate();
  const columns = [
    {
      key: 'product',
      header: 'Product',
      render: (r) => (
        <span>
          {r.product?.name ?? 'Unknown product'}{' '}
          <span className="num" style={{ color: 'var(--color-text-faint)', fontSize: 'var(--text-xs)' }}>
            {r.product?.sku}
          </span>
        </span>
      ),
    },
    { key: 'warehouse', header: 'Warehouse', hideBelow: 'sm', render: (r) => r.warehouse?.name ?? '—' },
    { key: 'available', header: 'Available', align: 'right', mono: true },
    { key: 'level', header: 'Level', render: (r) => <StockLevelBadge available={r.available} /> },
  ];

  return (
    <Table
      caption="Low stock"
      columns={columns}
      rows={rows ?? []}
      rowKey="stockId"
      loading={loading}
      error={error}
      onRetry={onRetry}
      onRowClick={(r) => navigate(`/stock/${r.stockId}`, { state: { stock: r } })}
      dense
      emptyState={
        <EmptyState
          compact
          title="Nothing running low"
          description={`Every stocked item has more than ${LOW_STOCK_THRESHOLD} available.`}
        />
      }
    />
  );
}
