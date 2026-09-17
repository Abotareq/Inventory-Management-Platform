import { Link } from 'react-router-dom';
import Table from '../common/Table';
import { formatCurrency } from '../../utils/formatCurrency';
import { shortId } from '../../utils/formatId';
import styles from './OrderDetail.module.css';

// Line items and totals for one order. Totals come from the server; nothing is recomputed here.
export default function OrderDetail({ order, productsById, warehousesById }) {
  const columns = [
    {
      key: 'productId',
      header: 'Product',
      render: (row) => {
        const p = productsById?.[row.productId];
        return (
          <div className={styles.cell}>
            <span className={styles.name}>{p?.name ?? 'Unknown product'}</span>
            <span className={[styles.sub, 'num'].join(' ')}>{p?.sku ?? shortId(row.productId)}</span>
          </div>
        );
      },
    },
    {
      key: 'warehouseId',
      header: 'Warehouse',
      hideBelow: 'md',
      render: (row) => {
        const w = warehousesById?.[row.warehouseId];
        return w ? (
          <Link to={`/stock?warehouseId=${row.warehouseId}`}>{w.name}</Link>
        ) : (
          <span className={styles.sub}>{shortId(row.warehouseId)}</span>
        );
      },
    },
    { key: 'quantity', header: 'Qty', align: 'right', mono: true },
    {
      key: 'unitPriceSnapshot',
      header: 'Unit price',
      align: 'right',
      mono: true,
      hideBelow: 'sm',
      render: (row) => formatCurrency(row.unitPriceSnapshot),
    },
    {
      key: 'lineTotal',
      header: 'Line total',
      align: 'right',
      mono: true,
      render: (row) => formatCurrency(row.lineTotal),
    },
  ];

  return (
    <div className={styles.detail}>
      <Table
        caption="Order items"
        columns={columns}
        rows={order.items ?? []}
        rowKey="orderItemId"
        dense
      />
      <div className={styles.totals}>
        <span className={styles.totalLabel}>Order total</span>
        <span className={[styles.totalValue, 'num'].join(' ')}>{formatCurrency(order.totalAmount)}</span>
      </div>
    </div>
  );
}
