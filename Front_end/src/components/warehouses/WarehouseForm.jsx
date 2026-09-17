import { useState } from 'react';
import Button from '../common/Button';
import Input from '../common/Input';
import { extractErrorMessage, extractFieldErrors } from '../../services/apiClient';
import styles from './WarehouseForm.module.css';

// Create or edit a warehouse. `initial` present = edit mode.
export default function WarehouseForm({ initial, onSubmit, onCancel }) {
  const [values, setValues] = useState({
    name: initial?.name ?? '',
    location: initial?.location ?? '',
  });
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [fieldErrors, setFieldErrors] = useState({});
  const isEdit = Boolean(initial);

  const set = (field) => (e) => setValues((v) => ({ ...v, [field]: e.target.value }));

  async function handleSubmit(e) {
    e.preventDefault();
    const name = values.name.trim();
    const location = values.location.trim();
    const local = {};
    if (!name) local.name = 'Enter a warehouse name.';
    if (!location) local.location = 'Enter a location.';
    if (Object.keys(local).length > 0) {
      setFieldErrors(local);
      return;
    }
    setError(null);
    setFieldErrors({});
    setSubmitting(true);
    try {
      await onSubmit({ name, location });
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
      <Input
        label="Name"
        name="name"
        value={values.name}
        onChange={set('name')}
        error={fieldErrors.name}
        required
        autoFocus
        maxLength={100}
      />
      <Input
        label="Location"
        name="location"
        value={values.location}
        onChange={set('location')}
        error={fieldErrors.location}
        hint="City, site, or address as your team refers to it."
        required
        maxLength={200}
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
          {isEdit ? 'Save changes' : 'Create warehouse'}
        </Button>
      </div>
    </form>
  );
}
