import Button from '../common/Button';
import EmptyState from '../common/EmptyState';
import Table from '../common/Table';
import { shortId } from '../../utils/formatId';
import styles from './CategoryList.module.css';

export default function CategoryList({
  categories,
  loading,
  error,
  onRetry,
  canEdit,
  onCreate,
  onEdit,
  onDelete,
}) {
  const columns = [
    { key: 'name', header: 'Name' },
    {
      key: 'id',
      header: 'ID',
      mono: true,
      hideBelow: 'sm',
      render: (row) => (
        <span className={styles.id} title={row.id}>
          {shortId(row.id)}
        </span>
      ),
    },
  ];

  if (canEdit) {
    columns.push({
      key: 'actions',
      header: <span className="visually-hidden">Actions</span>,
      align: 'right',
      render: (row) => (
        <div className={styles.actions}>
          <Button size="sm" variant="ghost" onClick={() => onEdit(row)}>
            Rename
          </Button>
          <Button size="sm" variant="ghost" onClick={() => onDelete(row)}>
            Delete
          </Button>
        </div>
      ),
    });
  }

  return (
    <Table
      caption="Categories"
      columns={columns}
      rows={categories ?? []}
      rowKey="id"
      loading={loading}
      error={error}
      onRetry={onRetry}
      emptyState={
        <EmptyState
          compact
          title="No categories yet"
          description="Categories group products so they're easier to find. Create the first one to get started."
          action={
            canEdit ? (
              <Button variant="primary" onClick={onCreate}>
                Create category
              </Button>
            ) : null
          }
        />
      }
    />
  );
}
