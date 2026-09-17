import Button from '../common/Button';
import EmptyState from '../common/EmptyState';
import Table from '../common/Table';
import { formatCurrency, isUnpriced } from '../../utils/formatCurrency';
import styles from './ProductList.module.css';

export default function ProductList({
  products,
  categoriesById,
  loading,
  error,
  onRetry,
  canEdit,
  filtered = false,
  onCreate,
  onEdit,
  onDelete,
  onOpenStock,
}) {
  const columns = [
    {
      key: 'sku',
      header: 'SKU',
      mono: true,
      width: '1%',
      render: (row) => <span className={styles.sku}>{row.sku}</span>,
    },
    {
      key: 'name',
      header: 'Name',
      render: (row) => (
        <div className={styles.nameCell}>
          <span className={styles.name}>{row.name}</span>
          {row.description && <span className={styles.description}>{row.description}</span>}
        </div>
      ),
    },
    {
      key: 'categoryId',
      header: 'Category',
      hideBelow: 'md',
      render: (row) =>
        row.categoryId ? (
          categoriesById?.[row.categoryId]?.name ?? <span className={styles.dim}>Unknown</span>
        ) : (
          <span className={styles.dim}>Uncategorised</span>
        ),
    },
    {
      key: 'price',
      header: 'Price',
      align: 'right',
      mono: true,
      render: (row) =>
        isUnpriced(row.price) ? (
          <span className={styles.dim} title="No price has been set for this product">
            No price
          </span>
        ) : (
          formatCurrency(row.price)
        ),
    },
    {
      key: 'actions',
      header: <span className="visually-hidden">Actions</span>,
      align: 'right',
      render: (row) => (
        <div className={styles.actions}>
          <Button size="sm" variant="ghost" onClick={() => onOpenStock(row)}>
            Stock
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

  const emptyState = filtered ? (
    <EmptyState
      compact
      title="No products match"
      description="Nothing on this page matches your filter. Clear it or move to another page."
    />
  ) : (
    <EmptyState
      compact
      title="No products yet"
      description={
        canEdit
          ? 'Products are what you stock and sell. Create the first one to get started.'
          : 'Products are what you stock and sell. An administrator needs to create them.'
      }
      action={
        canEdit ? (
          <Button variant="primary" onClick={onCreate}>
            Create product
          </Button>
        ) : null
      }
    />
  );

  return (
    <Table
      caption="Products"
      columns={columns}
      rows={products ?? []}
      rowKey="id"
      loading={loading}
      error={error}
      onRetry={onRetry}
      emptyState={emptyState}
    />
  );
}
