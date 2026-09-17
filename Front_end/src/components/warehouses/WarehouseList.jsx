import Button from '../common/Button';
import EmptyState from '../common/EmptyState';
import Table from '../common/Table';
import { shortId } from '../../utils/formatId';
import styles from './WarehouseList.module.css';

export default function WarehouseList({
  warehouses,
  loading,
  error,
  onRetry,
  canEdit,
  onCreate,
  onEdit,
  onDelete,
  onOpenStock,
}) {
  const columns = [
    { key: 'name', header: 'Name' },
    { key: 'location', header: 'Location', render: (row) => row.location || '—' },
    {
      key: 'id',
      header: 'ID',
      mono: true,
      hideBelow: 'md',
      render: (row) => (
        <span className={styles.id} title={row.id}>
          {shortId(row.id)}
        </span>
      ),
    },
    {
      key: 'actions',
      header: <span className="visually-hidden">Actions</span>,
      align: 'right',
      render: (row) => (
        <div className={styles.actions}>
          <Button size="sm" variant="ghost" onClick={() => onOpenStock(row)}>
            View stock
          </Button>
          {canEdit && (
            <>
              <Button size="sm" variant="ghost" onClick={() => onEdit(row)}>
                Edit
              </Button>
              <Button size="sm" variant="ghost" onClick={() => onDelete(row)}>
                Delete
              </Button>
            </>
          )}
        </div>
      ),
    },
  ];

  return (
    <Table
      caption="Warehouses"
      columns={columns}
      rows={warehouses ?? []}
      rowKey="id"
      loading={loading}
      error={error}
      onRetry={onRetry}
      emptyState={
        <EmptyState
          compact
          title="No warehouses yet"
          description={
            canEdit
              ? 'Stock lives in warehouses. Create the first one so products can be assigned to it.'
              : 'Stock lives in warehouses. An administrator needs to create one before products can be assigned.'
          }
          action={
            canEdit ? (
              <Button variant="primary" onClick={onCreate}>
                Create warehouse
              </Button>
            ) : null
          }
        />
      }
    />
  );
}
