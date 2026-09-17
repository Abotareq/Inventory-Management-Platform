import Button from './Button';
import styles from './Pagination.module.css';

export default function Pagination({ pageNumber, pageSize, totalCount, onPageChange }) {
  const total = Number(totalCount) || 0;
  const totalPages = Math.max(1, Math.ceil(total / pageSize));
  const from = total === 0 ? 0 : (pageNumber - 1) * pageSize + 1;
  const to = Math.min(pageNumber * pageSize, total);

  if (total <= pageSize && pageNumber === 1) {
    return (
      <nav className={styles.bar} aria-label="Pagination">
        <span className={styles.summary}>
          <span className="num">{total}</span> {total === 1 ? 'item' : 'items'}
        </span>
      </nav>
    );
  }

  return (
    <nav className={styles.bar} aria-label="Pagination">
      <span className={styles.summary}>
        Showing <span className="num">{from}–{to}</span> of <span className="num">{total}</span>
      </span>
      <div className={styles.controls}>
        <Button size="sm" onClick={() => onPageChange(pageNumber - 1)} disabled={pageNumber <= 1}>
          Previous
        </Button>
        <span className={styles.page}>
          Page <span className="num">{pageNumber}</span> of <span className="num">{totalPages}</span>
        </span>
        <Button
          size="sm"
          onClick={() => onPageChange(pageNumber + 1)}
          disabled={pageNumber >= totalPages}
        >
          Next
        </Button>
      </div>
    </nav>
  );
}
