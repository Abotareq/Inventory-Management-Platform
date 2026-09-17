import { useState } from 'react';
import Button from '../common/Button';
import Input from '../common/Input';
import Select from '../common/Select';
import { ALL_ROLES, ROLE_LABELS } from '../../config/constants';
import { extractErrorMessage, extractFieldErrors } from '../../services/apiClient';
import styles from './RegisterUserForm.module.css';

const EMPTY = { fullName: '', email: '', password: '', confirm: '', role: '' };

// Administrator-only: creates an employee account with one role.
export default function RegisterUserForm({ onSubmit, onCancel }) {
  const [values, setValues] = useState(EMPTY);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [fieldErrors, setFieldErrors] = useState({});

  const set = (field) => (e) => setValues((v) => ({ ...v, [field]: e.target.value }));

  function validate() {
    const local = {};
    if (!values.fullName.trim()) local.fullName = "Enter the employee's full name.";
    if (!values.email.trim()) local.email = 'Enter an email address.';
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(values.email.trim())) local.email = 'That does not look like an email address.';
    if (values.password.length < 8) local.password = 'Use at least 8 characters.';
    else if (!/[A-Z]/.test(values.password) || !/\d/.test(values.password) || !/[^A-Za-z0-9]/.test(values.password)) {
      local.password = 'Include an uppercase letter, a digit, and a symbol.';
    }
    if (values.confirm !== values.password) local.confirm = 'Passwords do not match.';
    if (!values.role) local.role = 'Choose a role.';
    return local;
  }

  async function handleSubmit(e) {
    e.preventDefault();
    const local = validate();
    setError(null);
    if (Object.keys(local).length > 0) {
      setFieldErrors(local);
      return;
    }
    setFieldErrors({});
    setSubmitting(true);
    try {
      await onSubmit({
        fullName: values.fullName.trim(),
        email: values.email.trim(),
        password: values.password,
        role: values.role,
      });
      setValues(EMPTY);
    } catch (err) {
      const fields = extractFieldErrors(err);
      // The API keys role errors as "User.InvalidRole"; everything else matches a field.
      if (fields['user.InvalidRole']) fields.role = fields['user.InvalidRole'];
      setFieldErrors(fields);
      if (!fields.fullName && !fields.email && !fields.password && !fields.role) {
        setError(extractErrorMessage(err));
      }
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <form className={styles.form} onSubmit={handleSubmit} noValidate>
      <Input
        label="Full name"
        name="fullName"
        autoComplete="off"
        value={values.fullName}
        onChange={set('fullName')}
        error={fieldErrors.fullName}
        required
        autoFocus
        maxLength={100}
      />
      <Input
        label="Email"
        name="email"
        type="email"
        autoComplete="off"
        value={values.email}
        onChange={set('email')}
        error={fieldErrors.email}
        hint="Used to sign in. Must be unique."
        required
      />
      <Select
        label="Role"
        name="role"
        value={values.role}
        onChange={set('role')}
        error={fieldErrors.role}
        placeholder="Choose a role"
        options={ALL_ROLES.map((r) => ({ value: r, label: ROLE_LABELS[r] }))}
        hint="Administrator: full setup. Warehouse operator: stock and fulfilment. Sales agent: orders. Manager: read-only."
        required
      />
      <div className={styles.row}>
        <Input
          label="Temporary password"
          name="password"
          type="password"
          autoComplete="new-password"
          value={values.password}
          onChange={set('password')}
          error={fieldErrors.password}
          hint="8+ characters with an uppercase letter, a digit, and a symbol."
          required
          className={styles.half}
        />
        <Input
          label="Confirm password"
          name="confirm"
          type="password"
          autoComplete="new-password"
          value={values.confirm}
          onChange={set('confirm')}
          error={fieldErrors.confirm}
          required
          className={styles.half}
        />
      </div>
      {error && (
        <p className={styles.error} role="alert">
          {error}
        </p>
      )}
      <div className={styles.actions}>
        {onCancel && (
          <Button variant="ghost" onClick={onCancel} disabled={submitting}>
            Cancel
          </Button>
        )}
        <Button type="submit" variant="primary" loading={submitting}>
          Register user
        </Button>
      </div>
    </form>
  );
}
