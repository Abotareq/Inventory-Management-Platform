import { useRef, useState } from 'react';
import Button from '../common/Button';
import Input from '../common/Input';
import Select from '../common/Select';
import { extractErrorMessage, extractFieldErrors } from '../../services/apiClient';
import { createIdempotencyKey } from '../../services/orderService';
import { formatCurrency, isUnpriced } from '../../utils/formatCurrency';
import styles from './CreateOrderForm.module.css';

const GUID = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;

let rowCounter = 0;
const newRow = () => ({ key: ++rowCounter, productId: '', warehouseId: '', quantity: '1' });

// Builds the CreateOrder payload. One idempotency key per submission attempt: a
// retry of the same payload reuses the key, an edited payload gets a fresh one.
export default function CreateOrderForm({ products, warehouses, onSubmit, onCancel }) {
  const [customerId, setCustomerId] = useState('');
  const [rows, setRows] = useState(() => [newRow()]);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [fieldErrors, setFieldErrors] = useState({});
  const attempt = useRef({ fingerprint: null, key: null });

  const productsById = Object.fromEntries((products ?? []).map((p) => [p.id, p]));

  function updateRow(key, field, value) {
    setRows((rs) => rs.map((r) => (r.key === key ? { ...r, [field]: value } : r)));
  }

  function removeRow(key) {
    setRows((rs) => (rs.length > 1 ? rs.filter((r) => r.key !== key) : rs));
  }

  function validate() {
    const local = {};
    if (!customerId.trim()) local.customerId = 'Enter a customer ID.';
    else if (!GUID.test(customerId.trim())) local.customerId = 'Customer IDs are GUIDs, like 3f2504e0-4f89-11d3-9a0c-0305e82c3301.';
    rows.forEach((r) => {
      if (!r.productId) local[`row-${r.key}-productId`] = 'Choose a product.';
      if (!r.warehouseId) local[`row-${r.key}-warehouseId`] = 'Choose a warehouse.';
      const q = Number(r.quantity);
      if (!Number.isInteger(q) || q <= 0) local[`row-${r.key}-quantity`] = 'Whole number, at least 1.';
    });
    const pairs = rows.map((r) => `${r.productId}|${r.warehouseId}`);
    rows.forEach((r, i) => {
      if (r.productId && r.warehouseId && pairs.indexOf(pairs[i]) !== i) {
        local[`row-${r.key}-productId`] = 'This product and warehouse are already on another line.';
      }
    });
    return local;
  }

  async function handleSubmit(e) {
    e.preventDefault();
    const local = validate();
    if (Object.keys(local).length > 0) {
      setFieldErrors(local);
      return;
    }
    setFieldErrors({});
    setError(null);

    const payload = {
      customerId: customerId.trim(),
      items: rows.map((r) => ({
        productId: r.productId,
        warehouseId: r.warehouseId,
        quantity: Number(r.quantity),
      })),
    };
    const fingerprint = JSON.stringify(payload);
    if (attempt.current.fingerprint !== fingerprint) {
      attempt.current = { fingerprint, key: createIdempotencyKey() };
    }

    setSubmitting(true);
    try {
      await onSubmit({ ...payload, idempotencyKey: attempt.current.key });
    } catch (err) {
      const fields = extractFieldErrors(err);
      if (fields.customerId) setFieldErrors({ customerId: fields.customerId });
      setError(extractErrorMessage(err));
    } finally {
      setSubmitting(false);
    }
  }

  const productOptions = (products ?? []).map((p) => ({ value: p.id, label: `${p.name} (${p.sku})` }));
  const warehouseOptions = (warehouses ?? []).map((w) => ({ value: w.id, label: w.name }));

  return (
    <form className={styles.form} onSubmit={handleSubmit} noValidate>
      <section className={styles.section}>
        <h2 className={styles.heading}>Customer</h2>
        <div className={styles.customerRow}>
          <Input
            label="Customer ID"
            value={customerId}
            onChange={(e) => setCustomerId(e.target.value)}
            error={fieldErrors.customerId}
            hint="Paste the customer's ID, or generate one for a new customer."
            mono
            required
            autoFocus
            className={styles.customer}
          />
          <Button
            variant="ghost"
            onClick={() => setCustomerId(crypto.randomUUID())}
            className={styles.generate}
          >
            Generate new ID
          </Button>
        </div>
      </section>

      <section className={styles.section}>
        <h2 className={styles.heading}>Items</h2>
        <div className={styles.rows}>
          {rows.map((row, index) => {
            const product = productsById[row.productId];
            return (
              <div key={row.key} className={styles.row}>
                <Select
                  label={index === 0 ? 'Product' : undefined}
                  aria-label={index === 0 ? undefined : `Product for line ${index + 1}`}
                  value={row.productId}
                  onChange={(e) => updateRow(row.key, 'productId', e.target.value)}
                  placeholder="Choose a product"
                  options={productOptions}
                  error={fieldErrors[`row-${row.key}-productId`]}
                  className={styles.product}
                />
                <Select
                  label={index === 0 ? 'Warehouse' : undefined}
                  aria-label={index === 0 ? undefined : `Warehouse for line ${index + 1}`}
                  value={row.warehouseId}
                  onChange={(e) => updateRow(row.key, 'warehouseId', e.target.value)}
                  placeholder="Choose a warehouse"
                  options={warehouseOptions}
                  error={fieldErrors[`row-${row.key}-warehouseId`]}
                  className={styles.warehouse}
                />
                <Input
                  label={index === 0 ? 'Qty' : undefined}
                  aria-label={index === 0 ? undefined : `Quantity for line ${index + 1}`}
                  type="number"
                  inputMode="numeric"
                  min="1"
                  step="1"
                  value={row.quantity}
                  onChange={(e) => updateRow(row.key, 'quantity', e.target.value)}
                  error={fieldErrors[`row-${row.key}-quantity`]}
                  mono
                  className={styles.qty}
                />
                <div className={[styles.price, index === 0 && styles.priceFirst].filter(Boolean).join(' ')}>
                  {product ? (
                    isUnpriced(product.price) ? (
                      <span className={styles.dim} title="This product has no price; its line total will be 0.">No price</span>
                    ) : (
                      <span className="num">{formatCurrency(product.price)}</span>
                    )
                  ) : (
                    <span className={styles.dim}>—</span>
                  )}
                </div>
                <div className={[styles.remove, index === 0 && styles.priceFirst].filter(Boolean).join(' ')}>
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => removeRow(row.key)}
                    disabled={rows.length === 1}
                    aria-label={`Remove line ${index + 1}`}
                  >
                    Remove
                  </Button>
                </div>
              </div>
            );
          })}
        </div>
        <Button variant="secondary" size="sm" onClick={() => setRows((rs) => [...rs, newRow()])}>
          Add line
        </Button>
        <p className={styles.hint}>
          Prices shown are current list prices. The order records a snapshot when it's created, and the server calculates the total.
        </p>
      </section>

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
          Create order
        </Button>
      </div>
    </form>
  );
}
