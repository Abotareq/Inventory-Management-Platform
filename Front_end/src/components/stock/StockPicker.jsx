import Select from '../common/Select';
import styles from './StockPicker.module.css';

// Choose the dimension to browse: one warehouse (rows = products) or one product (rows = warehouses).
export default function StockPicker({
  warehouses,
  products,
  warehouseId,
  productId,
  onWarehouseChange,
  onProductChange,
}) {
  return (
    <div className={styles.picker}>
      <Select
        label="Browse a warehouse"
        value={warehouseId ?? ''}
        onChange={(e) => onWarehouseChange(e.target.value)}
        placeholder="Choose a warehouse"
        options={(warehouses ?? []).map((w) => ({ value: w.id, label: `${w.name} — ${w.location}` }))}
        className={styles.select}
      />
      <span className={styles.or}>or</span>
      <Select
        label="Browse a product"
        value={productId ?? ''}
        onChange={(e) => onProductChange(e.target.value)}
        placeholder="Choose a product"
        options={(products ?? []).map((p) => ({ value: p.id, label: `${p.name} (${p.sku})` }))}
        className={styles.select}
      />
    </div>
  );
}
