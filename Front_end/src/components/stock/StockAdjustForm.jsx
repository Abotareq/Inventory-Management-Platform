import { useState } from 'react';
import Button from '../common/Button';
import Input from '../common/Input';
import Select from '../common/Select';
import { extractErrorMessage, extractFieldErrors } from '../../services/apiClient';
import styles from './StockForms.module.css';

// Signed adjustment against one stock record. Direction + amount → signed `amount`.
export default function StockAdjustForm({ stock, product, warehouse, onSubmit, onCancel }) {
  const [direction, setDirection] = useState('add');
  const [amount, setAmount] = useState('');
  const [reason, setReason] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [fieldErrors, setFieldErrors] = useState({});

  const qty = Number(amount);
  const signed = direction === 'remove' ? -qty : qty;
  const resulting = Number.isFinite(qty) && qty > 0 ? stock.quantity + signed : null;

  async function handleSubmit(e) {
    e.preventDefault();
    const local = {};
    if (!Number.isInteger(qty) || qty <= 0) local.amount = 'Enter a whole number greater than 0.';
    if (!reason.trim()) local.reason = 'Give a reason so the history makes sense later.';
    if (Object.keys(local).length > 0) {
      setFieldErrors(local);
      return;
    }
    setError(null);
    setFieldErrors({});
    setSubmitting(true);
    try {
      await onSubmit({
        productId: stock.productId,
        warehouseId: stock.warehouseId,
        amount: signed,
        reason: reason.trim(),
      });
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
      <dl className={styles.context}>
        <dt>Product</dt>
        <dd>
          {product?.name ?? 'Unknown product'}{' '}
          <span className="num">{product?.sku ? `(${product.sku})` : ''}</span>
        </dd>
        <dt>Warehouse</dt>
        <dd>{warehouse?.name ?? 'Unknown warehouse'}</dd>
        <dt>On hand</dt>
        <dd className="num">
          {stock.quantity} <span className={styles.hint}>({stock.reserved} reserved, {stock.available} available)</span>
        </dd>
      </dl>

      <div className={styles.row}>
        <Select
          label="Direction"
          value={direction}
          onChange={(e) => setDirection(e.target.value)}
          options={[
            { value: 'add', label: 'Add' },
            { value: 'remove', label: 'Remove' },
          ]}
          className={styles.direction}
        />
        <Input
          label="Amount"
          type="number"
          inputMode="numeric"
          min="1"
          step="1"
          value={amount}
          onChange={(e) => setAmount(e.target.value)}
          error={fieldErrors.amount}
          mono
          required
          autoFocus
          className={styles.amount}
        />
      </div>

      {resulting !== null && (
        <p className={styles.preview} aria-live="polite">
          On hand after this change:{' '}
          <span className={['num', resulting < 0 && styles.negative].filter(Boolean).join(' ')}>
            {resulting}
          </span>
          {resulting < 0 && ' — that takes stock below zero, so the server will reject it.'}
        </p>
      )}

      <Input
        label="Reason"
        value={reason}
        onChange={(e) => setReason(e.target.value)}
        error={fieldErrors.reason}
        multiline
        required
        maxLength={500}
        placeholder="Received delivery, cycle count correction, damaged goods…"
      />

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
          Adjust stock
        </Button>
      </div>
    </form>
  );
}
