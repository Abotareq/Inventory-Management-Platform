import { useId } from 'react';
import styles from './Input.module.css';

// Labelled text input. Set `multiline` for a textarea, `mono` for codes/numbers.
export default function Input({
  label,
  hint,
  error,
  multiline = false,
  mono = false,
  required = false,
  className,
  id: idProp,
  ...rest
}) {
  const autoId = useId();
  const id = idProp ?? autoId;
  const hintId = hint ? `${id}-hint` : undefined;
  const errorId = error ? `${id}-error` : undefined;
  const describedBy = [hintId, errorId].filter(Boolean).join(' ') || undefined;
  const Tag = multiline ? 'textarea' : 'input';

  return (
    <div className={[styles.field, className].filter(Boolean).join(' ')}>
      {label && (
        <label htmlFor={id} className={styles.label}>
          {label}
          {required && <span className={styles.required} aria-hidden="true"> *</span>}
        </label>
      )}
      <Tag
        id={id}
        className={[
          styles.control,
          multiline && styles.textarea,
          mono && styles.mono,
          error && styles.invalid,
        ]
          .filter(Boolean)
          .join(' ')}
        aria-invalid={error ? true : undefined}
        aria-describedby={describedBy}
        required={required}
        {...rest}
      />
      {hint && !error && (
        <p id={hintId} className={styles.hint}>
          {hint}
        </p>
      )}
      {error && (
        <p id={errorId} className={styles.error} role="alert">
          {error}
        </p>
      )}
    </div>
  );
}
