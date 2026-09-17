import styles from './PageHeader.module.css';

// Title row for every page. `actions` sits on the right; `meta` under the title.
export default function PageHeader({ title, meta, actions, children }) {
  return (
    <div className={styles.header}>
      <div className={styles.text}>
        <h1 className={styles.title}>{title}</h1>
        {meta && <div className={styles.meta}>{meta}</div>}
      </div>
      {actions && <div className={styles.actions}>{actions}</div>}
      {children}
    </div>
  );
}
