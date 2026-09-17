import { useState } from 'react';
import Button from '../common/Button';
import Input from '../common/Input';
import Select from '../common/Select';
import { extractErrorMessage, extractFieldErrors } from '../../services/apiClient';
import styles from './ProductForm.module.css';

// Create or edit a product. `initial` present = edit mode.
export default function ProductForm({ initial, categories, onSubmit, onCancel }) {
  const [values, setValues] = useState({
    name: initial?.name ?? '',
    sku: initial?.sku ?? '',
    description: initial?.description ?? '',
    categoryId: initial?.categoryId ?? '',
    price: initial?.price != null && initial.price !== 0 ? String(initial.price) : '',
  });
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [fieldErrors, setFieldErrors] = useState({});
  const isEdit = Boolean(initial);

  const set = (field) => (e) => setValues((v) => ({ ...v, [field]: e.target.value }));

  function validate() {
    const local = {};
    if (!values.name.trim()) local.name = 'Enter a product name.';
    if (!values.sku.trim()) local.sku = 'Enter a SKU.';
    else if (/\s/.test(values.sku.trim())) local.sku = 'SKUs cannot contain spaces.';
    if (values.price !== '') {
      const n = Number(values.price);
      if (!Number.isFinite(n) || n < 0) local.price = 'Enter a price of 0 or more.';
    }
    return local;
  }

  async function handleSubmit(e) {
    e.preventDefault();
    const local = validate();
    if (Object.keys(local).length > 0) {
      setFieldErrors(local);
      return;
    }
    setError(null);
    setFieldErrors({});
    setSubmitting(true);
    try {
      await onSubmit({
        name: values.name.trim(),
        sku: values.sku.trim(),
        description: values.description.trim(),
        categoryId: values.categoryId || null,
        price: values.price === '' ? 0 : Number(values.price),
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
      <div className={styles.row}>
        <Input
          label="Name"
          name="name"
          value={values.name}
          onChange={set('name')}
          error={fieldErrors.name}
          required
          autoFocus
          maxLength={150}
          className={styles.grow}
        />
        <Input
          label="SKU"
          name="sku"
          value={values.sku}
          onChange={set('sku')}
          error={fieldErrors.sku}
          hint="Unique code, no spaces."
          mono
          required
          maxLength={50}
          className={styles.sku}
        />
      </div>
      <Input
        label="Description"
        name="description"
        value={values.description}
        onChange={set('description')}
        error={fieldErrors.description}
        multiline
        maxLength={1000}
      />
      <div className={styles.row}>
        <Select
          label="Category"
          name="categoryId"
          value={values.categoryId}
          onChange={set('categoryId')}
          error={fieldErrors.categoryId}
          placeholder="No category"
          options={(categories ?? []).map((c) => ({ value: c.id, label: c.name }))}
          className={styles.grow}
        />
        <Input
          label="Price"
          name="price"
          type="number"
          inputMode="decimal"
          min="0"
          step="0.01"
          value={values.price}
          onChange={set('price')}
          error={fieldErrors.price}
          hint="Leave blank for no price yet."
          mono
          className={styles.price}
        />
      </div>
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
          {isEdit ? 'Save changes' : 'Create product'}
        </Button>
      </div>
    </form>
  );
}
