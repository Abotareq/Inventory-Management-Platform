import { Link } from 'react-router-dom';
import styles from './StatTile.module.css';

// One headline number. `to` makes the tile a link; `tone` colours the value.
export default function StatTile({ label, value, hint, to, tone, loading }) {
  const body = (
    <>
      <span className={styles.label}>{label}</span>
      <span className={[styles.value, 'num', tone && styles[tone]].filter(Boolean).join(' ')}>
        {loading ? <span className={styles.skeleton} aria-label="Loading" /> : value ?? '—'}
      </span>
      {hint && <span className={styles.hint}>{hint}</span>}
    </>
  );

  return to ? (
    <Link to={to} className={[styles.tile, styles.link].join(' ')}>
      {body}
    </Link>
  ) : (
    <div className={styles.tile}>{body}</div>
  );
}
