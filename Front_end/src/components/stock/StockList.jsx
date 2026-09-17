import Button from '../common/Button';
import EmptyState from '../common/EmptyState';
import Table from '../common/Table';
import StockLevelBadge from './StockLevelBadge';
import styles from './StockList.module.css';

// mode: 'warehouse' (rows are products in one warehouse) | 'product' (rows are warehouses holding one product)
export default function StockList({
  mode,
  stocks,
  productsById,
  warehousesById,
  loading,
  error,
  onRetry,
  canAdjust,
  canDelete,
  onAdjust,
  onDelete,
  onOpenDetail,
  emptyAction,
}) {
  const subjectColumn =
    mode === 'warehouse'
      ? {
          key: 'productId',
          header: 'Product',
          render: (row) => {
            const p = productsById?.[row.productId];
            return (
              <div className={styles.subject}>
                <span className={styles.name}>{p?.name ?? 'Unknown product'}</span>
                <span className={[styles.sub, 'num'].join(' ')}>{p?.sku ?? row.productId}</span>
              </div>
            );
          },
        }
      : {
          key: 'warehouseId',
          header: 'Warehouse',
          render: (row) => {
            const w = warehousesById?.[row.warehouseId];
            return (
              <div className={styles.subject}>
                <span className={styles.name}>{w?.name ?? 'Unknown warehouse'}</span>
                <span className={styles.sub}>{w?.location ?? row.warehouseId}</span>
              </div>
            );
          },
        };

  const columns = [
    subjectColumn,
    { key: 'quantity', header: 'On hand', align: 'right', mono: true },
    { key: 'reserved', header: 'Reserved', align: 'right', mono: true, hideBelow: 'sm' },
    {
      key: 'available',
      header: 'Available',
      align: 'right',
      mono: true,
      render: (row) => <strong>{row.available}</strong>,
    },
    {
      key: 'level',
      header: 'Level',
      hideBelow: 'md',
      render: (row) => <StockLevelBadge available={row.available} />,
    },
    {
      key: 'actions',
      header: <span className="visually-hidden">Actions</span>,
      align: 'right',
      render: (row) => (
        <div className={styles.actions}>
          <Button size="sm" variant="ghost" onClick={() => onOpenDetail(row)}>
            History
          </Button>
          {canAdjust && (
            <Button size="sm" variant="ghost" onClick={() => onAdjust(row)}>
              Adjust
            </Button>
          )}
          {canDelete && (
            <Button size="sm" variant="ghost" onClick={() => onDelete(row)}>
              Delete
            </Button>
          )}
        </div>
      ),
    },
  ];

  return (
    <Table
      caption={mode === 'warehouse' ? 'Stock in this warehouse' : 'Stock for this product'}
      columns={columns}
      rows={stocks ?? []}
      rowKey="stockId"
      loading={loading}
      error={error}
      onRetry={onRetry}
      emptyState={
        <EmptyState
          compact
          title={mode === 'warehouse' ? 'Nothing stocked here yet' : 'Not stocked anywhere yet'}
          description={
            mode === 'warehouse'
              ? 'Assign a product to this warehouse to start tracking its quantity.'
              : 'Assign this product to a warehouse to start tracking its quantity.'
          }
          action={emptyAction}
        />
      }
    />
  );
}
