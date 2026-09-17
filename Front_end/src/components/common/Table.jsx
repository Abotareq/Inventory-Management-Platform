import LoadingSpinner from './LoadingSpinner';
import ErrorMessage from './ErrorMessage';
import styles from './Table.module.css';

// columns: [{ key, header, render?(row, index), align?: 'left'|'right', mono?, width?, hideBelow?: 'md' }]
// rows: array of records; rowKey: string field name or (row) => key
// Pass `loading`, `error`/`onRetry`, or `emptyState` and Table renders them in place.
export default function Table({
  columns,
  rows = [],
  rowKey = 'id',
  caption,
  loading = false,
  error = null,
  onRetry,
  emptyState = null,
  onRowClick,
  dense = false,
  className,
}) {
  const getKey = typeof rowKey === 'function' ? rowKey : (row) => row[rowKey];
  const hasRows = rows.length > 0;

  if (error) {
    return <ErrorMessage title="Couldn't load this list" message={error} onRetry={onRetry} />;
  }

  return (
    <div className={[styles.wrapper, className].filter(Boolean).join(' ')}>
      <table className={[styles.table, dense && styles.dense].filter(Boolean).join(' ')}>
        {caption && <caption className="visually-hidden">{caption}</caption>}
        <thead>
          <tr>
            {columns.map((col) => (
              <th
                key={col.key}
                scope="col"
                className={[
                  col.align === 'right' && styles.right,
                  col.hideBelow && styles[`hide-${col.hideBelow}`],
                ]
                  .filter(Boolean)
                  .join(' ')}
                style={col.width ? { width: col.width } : undefined}
              >
                {col.header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {loading && !hasRows && (
            <tr>
              <td colSpan={columns.length} className={styles.stateCell}>
                <LoadingSpinner />
              </td>
            </tr>
          )}
          {!loading && !hasRows && emptyState && (
            <tr>
              <td colSpan={columns.length} className={styles.stateCell}>
                {emptyState}
              </td>
            </tr>
          )}
          {hasRows &&
            rows.map((row, index) => {
              const key = getKey(row) ?? index;
              const clickable = typeof onRowClick === 'function';
              return (
                <tr
                  key={key}
                  className={[clickable && styles.clickable, loading && styles.stale]
                    .filter(Boolean)
                    .join(' ')}
                  onClick={clickable ? () => onRowClick(row) : undefined}
                  onKeyDown={
                    clickable
                      ? (e) => {
                          if (e.key === 'Enter' || e.key === ' ') {
                            e.preventDefault();
                            onRowClick(row);
                          }
                        }
                      : undefined
                  }
                  tabIndex={clickable ? 0 : undefined}
                >
                  {columns.map((col) => (
                    <td
                      key={col.key}
                      className={[
                        col.align === 'right' && styles.right,
                        col.mono && 'num',
                        col.hideBelow && styles[`hide-${col.hideBelow}`],
                      ]
                        .filter(Boolean)
                        .join(' ')}
                    >
                      {col.render ? col.render(row, index) : row[col.key]}
                    </td>
                  ))}
                </tr>
              );
            })}
        </tbody>
      </table>
    </div>
  );
}
