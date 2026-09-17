import { useState } from 'react';
import Button from '../common/Button';
import Select from '../common/Select';
import { extractErrorMessage, extractFieldErrors } from '../../services/apiClient';
import styles from './StockForms.module.css';

// Creates a stock record (quantity 0) for a product + warehouse pair.
export default function StockAssignForm({ products, warehouses, initial, onSubmit, onCancel }) {
  const [productId, setProductId] = useState(initial?.productId ?? '');
  const [warehouseId, setWarehouseId] = useState(initial?.warehouseId ?? '');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [fieldErrors, setFieldErrors] = useState({});

  async function handleSubmit(e) {
    e.preventDefault();
    const local = {};
    if (!productId) local.productId = 'Choose a product.';
    if (!warehouseId) local.warehouseId = 'Choose a warehouse.';
    if (Object.keys(local).length > 0) {
      setFieldErrors(local);
      return;
    }
    setError(null);
    setFieldErrors({});
    setSubmitting(true);
    try {
      await onSubmit({ productId, warehouseId });
    } catch (err) {
      const fields = extractFieldErrors(err);
      setFieldErrors(fields);
      if (Object.keys(fields).length === 0) setError(extractErrorMessage(err));
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <form className={styles.form} onSubmit={handleSubmit} noValidate>
      <Select
        label="Product"
        value={productId}
        onChange={(e) => setProductId(e.target.value)}
        placeholder="Choose a product"
        options={(products ?? []).map((p) => ({ value: p.id, label: `${p.name} (${p.sku})` }))}
        error={fieldErrors.productId}
        required
        autoFocus
      />
      <Select
        label="Warehouse"
        value={warehouseId}
        onChange={(e) => setWarehouseId(e.target.value)}
        placeholder="Choose a warehouse"
        options={(warehouses ?? []).map((w) => ({ value: w.id, label: `${w.name} — ${w.location}` }))}
        error={fieldErrors.warehouseId}
        required
      />
      <p className={styles.hint}>The record starts at quantity 0. A warehouse operator adjusts it from there.</p>
      {error && (
        <p className={styles.error} role="alert">
          {error}
        </p>
      )}
      <div className={styles.actions}>
        <Button variant="ghost" onClick={onCancel} disabled={submitting}>
          Cancel
        </Button>
        <Button type="submit" variant="primary" loading={submitting}>
          Assign product
        </Button>
      </div>
    </form>
  );
}
