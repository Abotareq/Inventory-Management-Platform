import { useState } from 'react';
import Button from '../common/Button';
import Input from '../common/Input';
import { extractErrorMessage, extractFieldErrors } from '../../services/apiClient';
import styles from './CategoryForm.module.css';

// Create or rename a category. `initial` present = edit mode.
export default function CategoryForm({ initial, onSubmit, onCancel }) {
  const [name, setName] = useState(initial?.name ?? '');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [fieldErrors, setFieldErrors] = useState({});
  const isEdit = Boolean(initial);

  async function handleSubmit(e) {
    e.preventDefault();
    const trimmed = name.trim();
    if (!trimmed) {
      setFieldErrors({ name: 'Enter a category name.' });
      return;
    }
    setError(null);
    setFieldErrors({});
    setSubmitting(true);
    try {
      await onSubmit({ name: trimmed });
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
        value={name}
        onChange={(e) => setName(e.target.value)}
        error={fieldErrors.name}
        required
        autoFocus
        maxLength={100}
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
          {isEdit ? 'Save changes' : 'Create category'}
        </Button>
      </div>
    </form>
  );
}
